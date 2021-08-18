using Spear.Inf.Core.CusEnum;

namespace Spear.Inf.Core.CusException
{
    public class Exception_NoLogin : Exception_Base
    {
        public Exception_NoLogin() : base("请登录")
        {
            ECode = Enum_StateCode.NoLogin;
        }
    }
}
