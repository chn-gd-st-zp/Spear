using Spear.Inf.Core.Attr;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.CusException
{
    [LogIgnore]
    public class Exception_VerifyError : Exception_Base
    {
        public Exception_VerifyError(string msg) :base(msg)
        {
            ECode = ServiceContext.Resolve<IStateCode>().ValidError;
        }
    }
}
