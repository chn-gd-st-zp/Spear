using Spear.Inf.Core.Attr;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.CusException
{
    [LogIgnore]
    public class Exception_NoLogin : Exception_Base
    {
        public Exception_NoLogin() : base("请登录")
        {
            ECode = ISpearEnum.Restore<IStateCode>().NoLogin;
        }
    }
}
