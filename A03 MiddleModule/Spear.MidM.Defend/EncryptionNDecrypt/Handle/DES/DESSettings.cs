using Spear.Inf.Core.Attr;
using Spear.Inf.Core.Interface;

namespace Spear.MidM.Defend.EncryptionNDecrypt
{
    [DIModeForSettings("DESSettings", typeof(DESSettings))]
    public class DESSettings : IEncryptionNDecryptSettings
    {
        public DESSettings() { }

        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; } = "1234567890123456";

        /// <summary>
        /// 向量/偏移量
        /// </summary>
        public string IV { get; set; } = "1234567890123456";
    }
}
