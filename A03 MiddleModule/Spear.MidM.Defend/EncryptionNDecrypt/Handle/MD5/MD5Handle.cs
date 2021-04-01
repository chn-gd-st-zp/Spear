using System;

using Spear.Inf.Core.EncryptionNDecrypt;

namespace Spear.MidM.Defend.EncryptionNDecrypt
{
    public class MD5Handle : BasicHandle<MD5Settings>
    {
        public MD5Handle(string secretPrefix) : base(secretPrefix) { }

        public override string Encrypt(string text)
        {
            if (Settings == null)
                return MD5.Encrypt(SecretPrefix + text);

            return MD5.Encrypt(SecretPrefix + text, Settings.Digit);

            throw null;
        }

        public override string Decrypt(string text)
        {
            throw new NotImplementedException();
        }
    }
}
