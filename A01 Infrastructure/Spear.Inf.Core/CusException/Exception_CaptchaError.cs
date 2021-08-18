using Spear.Inf.Core.CusEnum;

namespace Spear.Inf.Core.CusException
{
    public class Exception_CaptchaError : Exception_Base
    {
        public Exception_CaptchaError(string msg) :base(msg)
        {
            ECode = Enum_StateCode.CaptchaFailed;
        }
    }
}
