using System;
using System.Text;

namespace Spear.Inf.Core.Tool
{
    /// <summary>
    /// 唯一值 生成类
    /// </summary>
    public class Unique
    {
        /// <summary>
        /// 获取GUID
        /// </summary>
        /// <param name="replaceSplitCode">是否移除中间的'-'分隔符</param>
        /// <returns></returns>
        public static string GetGUID(bool replaceSplitCode = true)
        {
            string result = "";

            result = Guid.NewGuid().ToString();

            if (replaceSplitCode)
                result = result.Replace("-", "");

            return result;
        }

        /// <summary>
        /// 获取随机数生成器
        /// </summary>
        /// <returns></returns>
        public static Random GetRandom()
        {
            //return new Random(DateTime.Now.Millisecond * new Random().Next(1000));

            //long tick = DateTime.Now.Ticks;
            //return new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            return new Random(Guid.NewGuid().GetHashCode());
        }

        /// <summary>
        /// 生成随机码(纯数字)
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static string GetRandomCode1(int min, int max)
        {
            return GetRandom().Next(min, max + 1).ToString();
        }

        /// <summary>
        /// 生成随机码(纯数字)
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomCode2(int length)
        {
            length--;
            return GetRandom().Next((int)Math.Pow(10, length), (int)Math.Pow(10, length + 1) - 1).ToString();
        }

        /// <summary>
        /// 生成随机码，0-9，a-z,A-Z
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomCode3(int length)
        {
            char[] constant =
            {
                '0','1','2','3','4','5','6','7','8','9',
                'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
            };

            StringBuilder newRandom = new StringBuilder(length);
            Random random = GetRandom();
            for (int i = 0; i < length; i++)
                newRandom.Append(constant[random.Next(length)]);

            return newRandom.ToString();
        }

        /// <summary>
        /// 用于验证码卡的随机数，不包含：0 O I L o i l 这些容易混淆的字符
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomCode4(int length)
        {
            char[] constant =
            {
                '1','2','3','4','5','6','7','8','9',
                'a','b','c','d','e','f','g','h','j','k','m','n','p','q','r','s','t','u','v','w','x','y','z',
                'A','B','C','D','E','F','G','H','J','K','M','N','P','Q','R','S','T','U','V','W','X','Y','Z'
            };

            StringBuilder newRandom = new StringBuilder(constant.Length);
            Random random = GetRandom();
            for (int i = 0; i < length; i++)
                newRandom.Append(constant[random.Next(constant.Length)]);

            return newRandom.ToString();
        }

        /// <summary>
        /// 字母全部为大写的随机码，不包含：0 O I L  这些容易混淆的字符
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomCode5(int length)
        {
            char[] constant =
            {
                '1','2','3','4','5','6','7','8','9',
                'A','B','C','D','E','F','G','H','J','K','M','N','P','Q','R','S','T','U','V','W','X','Y','Z',
            };

            StringBuilder newRandom = new StringBuilder(constant.Length);
            Random random = GetRandom();
            for (int i = 0; i < length; i++)
                newRandom.Append(constant[random.Next(constant.Length)]);

            return newRandom.ToString();
        }
    }
}
