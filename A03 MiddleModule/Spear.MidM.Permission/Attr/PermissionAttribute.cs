using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AspectInjector.Broker;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.Tool;
using Spear.MidM.SessionNAuth;

namespace Spear.MidM.Permission
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class PermissionBaseAttribute : Attribute
    {
        public Enum_PermissionType EType { get; }

        public string Code { get; }

        public string ParentCode { get; }

        public bool AccessLogger { get; }

        public Enum_Status EStatus { get; }

        public PermissionBaseAttribute(Enum_PermissionType eType, string code, bool accessLogger = true, Enum_Status eStatus = Enum_Status.Normal)
        {
            EType = eType;
            Code = code;
            ParentCode = "";
            AccessLogger = accessLogger;
            EStatus = eStatus;
        }

        public PermissionBaseAttribute(Enum_PermissionType eType, string code, string parentCode, bool accessLogger = true, Enum_Status eStatus = Enum_Status.Normal)
        {
            EType = eType;
            Code = code;
            ParentCode = parentCode;
            AccessLogger = accessLogger;
            EStatus = eStatus;
        }
    }

    [Aspect(Scope.PerInstance)]
    public class PermissionAspect : AOPAspectBase
    {
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
            var attrs = triggers.Where(o => o.IsExtendType<PermissionBaseAttribute>())
                .Select(o => o as PermissionBaseAttribute)
                .Where(o => o.EType == Enum_PermissionType.Action)
                .ToList();
            if (attrs == null || attrs.Count() == 0)
                return;

            var type = source.GetType();
            if (!type.IsImplementedType(typeof(IServiceWithTokenProvider<>)))
                return;

            var tp = ServiceContext.Resolve(type.GetGenericArguments()[0]) as ITokenProvider;
            var sha = ServiceContext.ResolveByKeyed<ISessionNAuth>(tp.Protocol);
            if (sha == null)
                return;

            foreach (var attr in attrs)
                sha.VerifyPermission(attr.Code);
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
