using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.CusException
{
    public class Exception_CaptchaError : Exception_Base
    {
        public Exception_CaptchaError(string msg) :base(msg)
        {
            ECode = ServiceContext.Resolve<IStateCode>().CaptchaFailed;
        }
    }
}
