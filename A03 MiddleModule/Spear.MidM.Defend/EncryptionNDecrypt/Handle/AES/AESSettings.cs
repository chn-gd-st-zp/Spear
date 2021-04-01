using System.Security.Cryptography;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

namespace Spear.MidM.Defend.EncryptionNDecrypt
{
    [DIModeForSettings("AESSettings", Enum_DIType.Specific, typeof(AESSettings))]
    public class AESSettings : IEncryptionNDecryptSettings
    {
        public AESSettings() { }

        /// <summary>
        /// 32位密钥
        /// </summary>
        public string Secret { get; set; } = "1234567890123456";

        /// <summary>
        /// 32位向量/偏移量
        /// </summary>
        public string IV { get; set; } = "abcdefghijklmnop";

        /// <summary>
        /// 加密模式
        /// </summary>
        public CipherMode ECipherMode { get; set; } = CipherMode.ECB;

        /// <summary>
        /// 填充模式
        /// </summary>
        public PaddingMode EPaddingMode { get; set; } = PaddingMode.PKCS7;
    }
}
