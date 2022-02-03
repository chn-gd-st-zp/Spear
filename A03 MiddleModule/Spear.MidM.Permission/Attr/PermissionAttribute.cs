using System;

using Spear.Inf.Core.CusEnum;

namespace Spear.MidM.Permission
{
    public abstract class PermissionBaseAttribute : Attribute
    {
        public Enum_PermissionType EType { get; }

        public string Code { get; }

        public string ParentCode { get; }

        public PermissionMappingType MappingType { get; }

        public bool AccessLogger { get; }

        public Enum_Status EStatus { get; } = Enum_Status.Normal;

        public PermissionBaseAttribute(Enum_PermissionType eType, string code, PermissionMappingType mappingType = null, bool? accessLogger = null)
        {
            EType = eType;
            Code = code;
            ParentCode = string.Empty;
            MappingType = mappingType;
            AccessLogger = mappingType != null;
            AccessLogger = accessLogger != null && accessLogger.HasValue ? accessLogger.Value : AccessLogger;
        }

        public PermissionBaseAttribute(Enum_PermissionType eType, string code, string parentCode, PermissionMappingType mappingType = null, bool? accessLogger = null)
        {
            EType = eType;
            Code = code;
            ParentCode = parentCode;
            MappingType = mappingType;
            AccessLogger = mappingType != null;
            AccessLogger = accessLogger != null && accessLogger.HasValue ? accessLogger.Value : AccessLogger;
        }

        public abstract IPermission Convert();
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public abstract class ClassPermissionBaseAttribute : PermissionBaseAttribute
    {
        public ClassPermissionBaseAttribute(string code)
            : base(Enum_PermissionType.Group, code, null, false) { }

        public ClassPermissionBaseAttribute(string code, string parentCode)
            : base(Enum_PermissionType.Group, code, parentCode, null, false) { }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class MethodPermissionBaseAttribute : PermissionBaseAttribute
    {
        public MethodPermissionBaseAttribute(string code, PermissionMappingType mappingType = null, bool? accessLogger = null)
            : base(Enum_PermissionType.Action, code, mappingType, accessLogger) { }

        public MethodPermissionBaseAttribute(string code, string parentCode, PermissionMappingType mappingType = null, bool? accessLogger = null)
            : base(Enum_PermissionType.Action, code, parentCode, mappingType, accessLogger) { }
    }
}
