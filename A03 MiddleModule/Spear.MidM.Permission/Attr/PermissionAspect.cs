using System;
using System.Linq;
using System.Reflection;

using AspectInjector.Broker;

using Spear.Inf.Core;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Permission
{

    [Aspect(Scope.PerInstance)]
    public class PermissionAspect : AOPAspectBase
    {
        private ISpearLogger<PermissionAspect> _logger;
        private IPermissionRepository _repository;

        public PermissionAspect()
        {
            _logger = ServiceContext.Resolve<ISpearLogger<PermissionAspect>>();
            _repository = ServiceContext.Resolve<IPermissionRepository>();
        }

        [Advice(Kind.Around)]
        public new object HandleMethod(
           [Argument(Source.Instance)] object source,
           [Argument(Source.Target)] Func<object[], object> method,
           [Argument(Source.Triggers)] Attribute[] triggers,
           [Argument(Source.Name)] string actionName,
           [Argument(Source.Arguments)] object[] actionParams
        )
        {
            return base.HandleMethod(source, method, triggers, actionName, actionParams);
        }

        protected override void Before(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams)
        {
            try
            {
                if (AppInitHelper.IsTestMode)
                    return;

                if (_repository == null)
                    return;

                var attr = triggers.Where(o => o.GetType().IsExtendOf<MethodPermissionBaseAttribute>())
                    .Select(o => o as MethodPermissionBaseAttribute).Where(o => o.EType == Enum_PermissionType.Action)
                    .FirstOrDefault();
                if (attr == null)
                    return;

                var type = source.GetType();
                if (!type.IsImplementedOf(typeof(IServiceWithTokenProvider<>)))
                    return;

                var tokenProvider = ServiceContext.Resolve(type.GetGenericArguments()[0]) as ITokenProvider;
                if (tokenProvider == null)
                    return;

                var session = ServiceContext.ResolveByKeyed<ISpearSession>(tokenProvider.Protocol);
                if (session == null)
                    return;

                var permission = _repository.Permission(attr.Code);
                if (permission == null || permission.EStatus != Enum_Status.Normal)
                    return;

                session.VerifyPermission(attr.Code);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        protected override void Error(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams, Exception error, out bool throwException)
        {
            throwException = true;
        }

        protected override object After(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams, object actionResult)
        {
            return actionResult;
        }
    }
}
