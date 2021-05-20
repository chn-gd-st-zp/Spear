using System;

namespace Spear.Inf.Core.Attr
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAuthAttribute : Attribute
    {
        public readonly string Code;

        public PermissionAuthAttribute(string code)
        {
            Code = code;
        }
    }
}
