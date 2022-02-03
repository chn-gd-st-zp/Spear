using System;

using Spear.Inf.Core.CusEnum;

namespace Spear.MidM.Permission
{
    public abstract class PermissionBaseAttribute : Attribute
    {
        public Enum_PermissionType EType { get; }

        public string Code { get; }

        public string ParentCode { get; }

        public bool AccessLogger { get; }

        public Enum_Status EStatus { get; }

        public PermissionBaseAttribute(Enum_PermissionType eType, string code, bool accessLogger = false, Enum_Status eStatus = Enum_Status.Normal)
        {
            EType = eType;
            Code = code;
            ParentCode = string.Empty;
            AccessLogger = accessLogger;
            EStatus = eStatus;
        }

        public PermissionBaseAttribute(Enum_PermissionType eType, string code, string parentCode, bool accessLogger = false, Enum_Status eStatus = Enum_Status.Normal)
        {
            EType = eType;
            Code = code;
            ParentCode = parentCode;
            AccessLogger = accessLogger;
            EStatus = eStatus;
        }

        public abstract IPermission Convert();
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public abstract class ClassPermissionBaseAttribute : PermissionBaseAttribute
    {
        public ClassPermissionBaseAttribute(string code, bool accessLogger = false, Enum_Status eStatus = Enum_Status.Normal) : base(Enum_PermissionType.Group, code, accessLogger, eStatus) { }

        public ClassPermissionBaseAttribute(string code, string parentCode, bool accessLogger = false, Enum_Status eStatus = Enum_Status.Normal) : base(Enum_PermissionType.Group, code, parentCode, accessLogger, eStatus) { }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class MethodPermissionBaseAttribute : PermissionBaseAttribute
    {
        public MethodPermissionBaseAttribute(string code, bool accessLogger = false, Enum_Status eStatus = Enum_Status.Normal) : base(Enum_PermissionType.Action, code, accessLogger, eStatus) { }

        public MethodPermissionBaseAttribute(string code, string parentCode, bool accessLogger = false, Enum_Status eStatus = Enum_Status.Normal) : base(Enum_PermissionType.Action, code, parentCode, accessLogger, eStatus) { }
    }
}
