using Spear.Inf.Core.Attr;

namespace Spear.Inf.Core.CusEnum
{
    public enum Enum_ContentType
    {
        [Remark("默认、无")]
        None,

        [Remark("application/x-www-form-urlencoded")]
        Form,

        [Remark("application/json")]
        Json,

        [Remark("application/xml")]
        Xml,

        [Remark("application/text")]
        Text,
    }

    public enum Enum_HttpMode
    {
        [Remark("默认、无")]
        None,

        [Remark("Get")]
        GET,

        [Remark("Post")]
        POST,
    }
}
