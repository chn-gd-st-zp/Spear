using System;

using Spear.Inf.Core.CusEnum;

namespace Spear.MidM.Permission
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class AccessLoggerBaseAttribute : Attribute
    {
        public string PermissionCode { get; }

        public Enum_OperationType EOperationType { get; }

        public Type InputType { get; }

        public Type DBDestinationType { get; }

        public AccessLoggerBaseAttribute(string permissionCode, Enum_OperationType eOperationType, Type inputType, Type dbDestinationType)
        {
            PermissionCode = permissionCode;
            EOperationType = eOperationType;
            InputType = inputType;
            DBDestinationType = dbDestinationType;
        }
    }
}
