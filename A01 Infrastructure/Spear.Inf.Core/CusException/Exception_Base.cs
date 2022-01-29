using System;

using Spear.Inf.Core.Base;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.CusException
{
    public class Exception_Base : Exception
    {
        public EnumInfo ECode { get; protected set; }

        public Exception_Base(string msg) : base(msg)
        {
            ECode = ServiceContext.Resolve<IStateCode>().None;
        }

        public Exception_Base(EnumInfo code, string msg) : base(msg)
        {
            ECode = code;
        }
    }
}
