using System;
using System.Security.Cryptography;
using System.Text;

namespace Spear.Inf.Core.EncryptionNDecrypt
{
    public class MD5
    {
        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="text">需要加密的字符串</param>
        /// <returns></returns>
        public static string Encrypt(string text)
        {
            System.Security.Cryptography.MD5 md5 = new MD5CryptoServiceProvider();
            byte[] res = md5.ComputeHash(Encoding.Default.GetBytes(text), 0, text.Length);

            string result = "";

            for (int i = 0; i < res.Length; i++)
                result += Uri.HexEscape((char)res[i]);

            result = result.Replace("%", "");
            result = result.ToLower();

            return result;
        }

        /// <summary>
        /// MD5 解密
        /// </summary>
        /// <param name="text">需要加密的字符串</param>
        /// <param name="digit">加密位数</param>
        /// <returns></returns>
        public static string Encrypt(string text, int digit = 32)
        {
            string md5code = string.Empty;
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] retVal = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
                StringBuilder md5sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    md5sb.Append(retVal[i].ToString("x2"));
                }
                md5code = md5sb.ToString();
            }

            if (digit == 16) //16位MD5加密（取32位加密的9~25字符）
            {
                return md5code.Substring(8, 16);
            }

            if (digit == 32) //32位加密
            {
                return md5code;
            }

            return md5code;
        }
    }
}
