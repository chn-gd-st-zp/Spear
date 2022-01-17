using Spear.Inf.Core.Attr;

namespace Spear.Demo.Inf
{
    public enum Enum_UserType
    {
        [Remark("默认、无")]
        None = 0,

        [Remark("代理")]
        Agent,

        [Remark("客户")]
        Customer,
    }
}
