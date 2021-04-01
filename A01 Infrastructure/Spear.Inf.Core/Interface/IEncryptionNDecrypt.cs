using Spear.Inf.Core.SettingsGeneric;

namespace Spear.Inf.Core.Interface
{
    public interface IEncryptionNDecrypt
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text">需要加密的字符串</param>
        /// <returns></returns>
        public string Encrypt(string text);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="text">需要解密的字符串</param>
        /// <returns></returns>
        public string Decrypt(string text);
    }

    public interface IEncryptionNDecryptHandle<TConfig> : IEncryptionNDecrypt where TConfig : IEncryptionNDecryptSettings
    {
        TConfig Settings { get; }
    }

    public interface IEncryptionNDecryptSettings : ISettings
    {
        //
    }
}
