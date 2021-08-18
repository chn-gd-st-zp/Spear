using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Defend.EncryptionNDecrypt
{
    public abstract class BaseHandle<TSettings> : IEncryptionNDecryptHandle<TSettings> where TSettings : class, IEncryptionNDecryptSettings, new()
    {
        protected string SecretPrefix { get; private set; }

        public TSettings Settings { get; set; }

        public BaseHandle(string secretPrefix) : base()
        {
            SecretPrefix = secretPrefix.IsEmptyString() ? "" : secretPrefix;
            Settings = ServiceContext.Resolve<TSettings>();
        }

        public abstract string Decrypt(string text);

        public abstract string Encrypt(string text);
    }
}
