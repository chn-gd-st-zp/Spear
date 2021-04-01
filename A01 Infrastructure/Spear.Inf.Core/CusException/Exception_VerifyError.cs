using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;

namespace Spear.Inf.Core.CusException
{
    [LogIgnore]
    public class Exception_VerifyError : Exception_Basic
    {
        public Exception_VerifyError(string msg) :base(msg)
        {
            ECode = Enum_StateCode.ValidError;
        }
    }
}
