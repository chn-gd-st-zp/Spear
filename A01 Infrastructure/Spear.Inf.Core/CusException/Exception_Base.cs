using System;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.CusException
{
    public class Exception_Base : Exception
    {
        public SpearEnumItem ECode { get; protected set; }

        public Exception_Base(string msg) : base(msg)
        {
            ECode = ISpearEnum.Restore<IStateCode>().None;
        }

        public Exception_Base(SpearEnumItem code, string msg) : base(msg)
        {
            ECode = code;
        }
    }
}
