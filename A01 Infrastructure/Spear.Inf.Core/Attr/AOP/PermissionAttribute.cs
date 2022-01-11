using System;
using System.Reflection;

using AspectInjector.Broker;

using Spear.Inf.Core.CusEnum;

namespace Spear.Inf.Core.Attr
{
    [Injection(typeof(PermissionAspect))]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAttribute : Attribute
    {
        public readonly Enum_PermissionType EType;

        public readonly string Code;

        public readonly string ParentCode;

        public readonly bool AccessLogger;

        public readonly Enum_Status EStatus;

        public PermissionAttribute(Enum_PermissionType eType, object code, bool accessLogger = true, Enum_Status eStatus = Enum_Status.Normal)
        {
            EType = eType;
            Code = code.ToString();
            ParentCode = "";
            AccessLogger = accessLogger;
            EStatus = eStatus;
        }

        public PermissionAttribute(Enum_PermissionType eType, object code, object parentCode, bool accessLogger = true, Enum_Status eStatus = Enum_Status.Normal)
        {
            EType = eType;
            Code = code.ToString();
            ParentCode = parentCode.ToString();
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
            //
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
