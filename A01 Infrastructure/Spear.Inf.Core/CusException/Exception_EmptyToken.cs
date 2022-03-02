using Spear.Inf.Core.Attr;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.CusException
{
    [LogIgnore]
    public class Exception_EmptyToken : Exception_Base
    {
        public Exception_EmptyToken() :base("空令牌")
        {
            ECode = ISpearEnum.Restore<IStateCode>().EmptyToken;
        }
    }
}
