using Spear.Inf.Core.CusEnum;

namespace Spear.Inf.Core.CusException
{
    public class Exception_EmptyToken : Exception_Basic
    {
        public Exception_EmptyToken() :base("空令牌")
        {
            ECode = Enum_StateCode.EmptyToken;
        }
    }
}
