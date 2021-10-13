using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

namespace Spear.MidM.Defend.EncryptionNDecrypt
{
    [DIModeForSettings("MD5Settings", Enum_DIType.Exclusive, typeof(MD5Settings))]
    public class MD5Settings : IEncryptionNDecryptSettings
    {
        public MD5Settings() { }

        /// <summary>
        /// 加密位数
        /// </summary>
        public int Digit { get; set; } = 32;
    }
}
