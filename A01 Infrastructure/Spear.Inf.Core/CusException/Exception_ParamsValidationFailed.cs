using Spear.Inf.Core.Attr;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.CusException
{
    [LogIgnore]
    public class Exception_ParamsValidationFailed : Exception_Base
    {
        public Exception_ParamsValidationFailed(string msg) :base(msg)
        {
            ECode = ServiceContext.Resolve<IStateCode>().ParamsValidationFailed;
        }
    }
}
