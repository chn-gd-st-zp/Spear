using System;

using Spear.Inf.Core.CusEnum;

namespace Spear.Inf.Core.CusException
{
    public class Exception_Base : Exception
    {
        public EnumInfo ECode { get; protected set; }

        public Exception_Base(string msg) : base(msg)
        {
            ECode = Enum_StateCode.None;
        }

        public Exception_Base(EnumInfo code, string msg) : base(msg)
        {
            ECode = code;
        }
    }
}
