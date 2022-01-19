using Spear.Inf.Core.Attr;

namespace Spear.MidM.MicoServ
{
    public enum Enum_AccessMode
    {
        [Remark("默认、无")]
        None,

        [Remark("公开")]
        Public,

        [Remark("内部")]
        Internal,

        [Remark("公开与内部")]
        PublicNInternal,
    }

    public enum Enum_RegisCenter
    {
        [Remark("默认、无")]
        None,

        [Remark("普通")]
        Normal,

        [Remark("Consul")]
        Consul,
    }
}
