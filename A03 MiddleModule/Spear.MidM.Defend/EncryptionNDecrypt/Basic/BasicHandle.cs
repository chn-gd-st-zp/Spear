using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Defend.EncryptionNDecrypt
{
    public abstract class BasicHandle<TSettings> : IEncryptionNDecryptHandle<TSettings> where TSettings : class, IEncryptionNDecryptSettings, new()
    {
        protected string SecretPrefix { get; private set; }

        public TSettings Settings { get; set; }

        public BasicHandle(string secretPrefix) : base()
        {
            SecretPrefix = secretPrefix.IsEmptyString() ? "" : secretPrefix;
            Settings = ServiceContext.ResolveServ<TSettings>();
        }

        public abstract string Decrypt(string text);

        public abstract string Encrypt(string text);
    }
}
