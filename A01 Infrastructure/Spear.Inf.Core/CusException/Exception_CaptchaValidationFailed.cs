using Spear.Inf.Core.Attr;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.CusException
{
    [LogIgnore]
    public class Exception_CaptchaValidationFailed : Exception_Base
    {
        public Exception_CaptchaValidationFailed(string msg) :base(msg)
        {
            ECode = ISpearEnum.Restore<IStateCode>().CaptchaValidationFailed;
        }
    }
}
