using Spear.Inf.Core.CusEnum;

namespace Spear.Inf.Core.CusException
{
    public class Exception_NoAuth : Exception_Base
    {
        public Exception_NoAuth() :base("没有权限")
        {
            ECode = Enum_StateCode.NoAuth;
        }
    }
}
