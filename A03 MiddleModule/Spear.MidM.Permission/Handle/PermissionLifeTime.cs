using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Spear.Inf.Core;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Injection;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Permission
{
    internal class PermissionLifeTime : ISpearAppLifeTime
    {
        private IPermissionInitialization _lifeTime;
        private IPermissionRepository _permissionRepository;

        internal PermissionLifeTime()
        {
            _lifeTime = ServiceContext.Resolve<IPermissionInitialization>();
            _permissionRepository = ServiceContext.Resolve<IPermissionRepository>();
        }

        public async Task Started(params object[] args)
        {
            var attrs = new List<PermissionBaseAttribute>();

            AppInitHelper
                .GetAllType(ServiceContext.Resolve<InjectionSettings>().Patterns)
                .Select(o => new
                {
                    Current = o,
                    PermissionAttrs = o.GetCustomAttributes().Where(oo => oo.GetType().IsExtendOf<PermissionBaseAttribute>()).Select(oo => oo as PermissionBaseAttribute).ToList(),
                })
                .Where(o => o.PermissionAttrs != null && o.PermissionAttrs.Count() != 0)
                .ToList()
                .ForEach(classObj =>
                {
                    foreach (var attr in classObj.PermissionAttrs)
                        attrs.Add(attr);

                    classObj.Current.GetMethods()
                        .Select(o => new
                        {
                            Current = o,
                            PermissionAttrs = o.GetCustomAttributes().Where(oo => oo.GetType().IsExtendOf<PermissionBaseAttribute>()).Select(ooo => ooo as PermissionBaseAttribute).ToList()
                        })
                        .Where(o => o.PermissionAttrs != null && classObj.PermissionAttrs.Count() != 0)
                        .ToList()
                        .ForEach(methodObj =>
                        {
                            foreach (var attr in methodObj.PermissionAttrs)
                                attrs.Add(attr);
                        });
                });

            attrs = attrs.GroupBy(o => o.Code).Select(o => o.First()).ToList();

            var permissionsFromService = attrs.Select(o => o.Convert()).ToList();
            var permissionsFromDB = _permissionRepository.AllPermission();

            _lifeTime.Operation(permissionsFromService, permissionsFromDB);

            if (permissionsFromDB.Count() != 0)
                _permissionRepository.Delete(permissionsFromDB);

            if (permissionsFromService.Count() != 0)
                _permissionRepository.Create(permissionsFromService);
        }

        public async Task Stopping(params object[] args) { }

        public async Task Stopped(params object[] args) { }
    }
}
