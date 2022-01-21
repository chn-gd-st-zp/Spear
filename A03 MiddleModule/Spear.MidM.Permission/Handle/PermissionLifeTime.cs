using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Spear.Inf.Core;
using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Injection;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Permission
{
    internal class PermissionLifeTime : ISpearAppLifeTime
    {
        private IPermissionLifeTime _lifeTime;
        private IPermissionRepository _permissionRepository;

        internal PermissionLifeTime()
        {
            _lifeTime = ServiceContext.Resolve<IPermissionLifeTime>();
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
                    PermissionAttrs = o.GetCustomAttributes().Where(oo => oo.IsExtendType<PermissionBaseAttribute>()).Select(oo => oo as PermissionBaseAttribute).ToList(),
                })
                .Where(o => o.PermissionAttrs != null && o.PermissionAttrs.Count() != 0)
                .ToList()
                .ForEach(cla =>
                {
                    foreach (var attr in cla.PermissionAttrs)
                        attrs.Add(attr);

                    cla.Current.GetMethods()
                        .Select(o => new
                        {
                            Current = o,
                            PermissionAttrs = o.GetCustomAttributes().Where(oo => oo.IsExtendType<PermissionBaseAttribute>()).Select(ooo => ooo as PermissionBaseAttribute).ToList()
                        })
                        .Where(o => o.PermissionAttrs != null && cla.PermissionAttrs.Count() != 0)
                        .ToList()
                        .ForEach(func =>
                        {
                            foreach (var attr in func.PermissionAttrs)
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
