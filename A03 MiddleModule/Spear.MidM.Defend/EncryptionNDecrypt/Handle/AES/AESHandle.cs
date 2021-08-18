using Spear.Inf.Core.EncryptionNDecrypt;

namespace Spear.MidM.Defend.EncryptionNDecrypt
{
    public class AESHandle : BaseHandle<AESSettings>
    {
        public AESHandle(string secretPrefix) : base(secretPrefix) { }

        public override string Encrypt(string text)
        {
            return AES.Encrypt(SecretPrefix + Settings.Secret, Settings.IV, Settings.ECipherMode, Settings.EPaddingMode, text);
        }

        public override string Decrypt(string text)
        {
            return AES.Decrypt(SecretPrefix + Settings.Secret, Settings.IV, Settings.ECipherMode, Settings.EPaddingMode, text);
        }
    }
}
