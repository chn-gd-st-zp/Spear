using Spear.Inf.Core.Attr;

namespace Spear.Inf.Core.CusEnum
{
    public enum Enum_Entry
    {
        [Remark("默认、无")]
        None = 0,

        [Remark("其他")]
        Other = 1,

        [Remark("PC")]
        PC = 2,

        [Remark("H5")]
        H5 = 3,

        [Remark("苹果APP")]
        IOSAPP = 4,

        [Remark("安卓APP")]
        AndroidAPP = 5,

        [Remark("微信小程序")]
        WeChatMicoApp = 6,
    }

    public enum Enum_AppType
    {
        [Remark("默认、无")]
        None = 0,

        [Remark("苹果")]
        IOS,

        [Remark("安卓")]
        Android,
    }

    public enum Enum_EncryptionNDecrypt
    {
        [Remark("默认、无")]
        None,

        [Remark("AES")]
        AES,

        [Remark("DES")]
        DES,

        [Remark("DES")]
        MD5,
    }

    public enum Enum_SIMProvider
    {
        [Remark("默认、无")]
        None,

        [Remark("中国电信")]
        ChinaTelecom,

        [Remark("中国移动")]
        ChinaMobile,

        [Remark("中国联通")]
        ChinaUnicom,

        [Remark("其他")]
        Other,
    }
}
