using System;

using Spear.Inf.Core.CusEnum;

namespace Spear.Inf.Core.Attr
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
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
}
