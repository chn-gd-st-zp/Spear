using System;

using Spear.Inf.Core.CusEnum;

namespace Spear.Inf.Core.CusException
{
    public class Exception_Basic : Exception
    {
        public EnumInfo ECode { get; protected set; }

        public Exception_Basic(string msg) : base(msg)
        {
            ECode = Enum_StateCode.None;
        }

        public Exception_Basic(EnumInfo code, string msg) : base(msg)
        {
            ECode = code;
        }
    }
}
