using Spear.Inf.Core.EncryptionNDecrypt;

namespace Spear.MidM.Defend.EncryptionNDecrypt
{
    public class DESHandle : BasicHandle<DESSettings>
    {
        public DESHandle(string secretPrefix) : base(secretPrefix) { }

        public override string Encrypt(string text)
        {
            return DES.Encrypt(SecretPrefix + Settings.Secret, Settings.IV, text);
        }

        public override string Decrypt(string text)
        {
            return DES.Decrypt(SecretPrefix + Settings.Secret, Settings.IV, text);
        }
    }
}
