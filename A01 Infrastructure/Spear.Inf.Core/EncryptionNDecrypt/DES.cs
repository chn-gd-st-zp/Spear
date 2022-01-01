using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.EncryptionNDecrypt
{
    public class DES
    {
        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="secret">密钥[8位，如：12345678]</param>
        /// <param name="iv">向量/偏移量[16位，如：abcdefghijklmnop]</param>  
        /// <param name="text">需要加密的字符串</param>
        /// <returns></returns>
        public static string Encrypt(string secret, string iv, string text)
        {
            secret = secret.Length < 8 ? secret.PadRight(8, ' ') : secret.Substring(0, 8);
            iv = iv.Length < 16 ? iv.PadRight(16, ' ') : iv.Substring(0, 16);

            byte[] rgbKey = Encoding.UTF8.GetBytes(secret);
            byte[] rgbIV = iv.ToByteArray();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(text);

            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();

            return Convert.ToBase64String(mStream.ToArray());
        }

        /// <summary>
        /// DES 解密
        /// </summary>
        /// <param name="secret">密钥[8位，如：12345678]</param>
        /// <param name="iv">向量/偏移量[16位，如：abcdefghijklmnop]</param>  
        /// <param name="text">需要解密的字符串</param>
        /// <returns></returns>
        public static string Decrypt(string secret, string iv, string text)
        {
            secret = secret.Length < 8 ? secret.PadRight(8, ' ') : secret.Substring(0, 8);
            iv = iv.Length < 16 ? iv.PadRight(16, ' ') : iv.Substring(0, 16);

            byte[] rgbKey = Encoding.UTF8.GetBytes(secret);
            byte[] rgbIV = iv.ToByteArray();
            byte[] inputByteArray = Convert.FromBase64String(text);

            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();

            return Encoding.UTF8.GetString(mStream.ToArray());
        }
    }
}
