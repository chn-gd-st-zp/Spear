using System;

namespace Spear.Inf.Core.Tool
{
    public static class MathConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dividend">被除数</param>
        /// <param name="divisor">除数（不能为0）</param>
        /// <param name="decimals">小数位数</param>
        /// <returns></returns>
        public static decimal Divider(decimal dividend, decimal divisor, int decimals = -1)
        {
            if (divisor == 0)
                return 0;

            decimal result = dividend / divisor;

            if (decimals == -1)
                return result;

            result = Math.Round(result, decimals);

            return result;
        }

        /// <summary>
        /// 转为正数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static decimal ToPositive(this decimal num)
        {
            return num >= 0 ? num : Math.Abs(num);
        }

        /// <summary>
        /// 转为负数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static decimal ToNegative(this decimal num)
        {
            return num >= 0 ? -num : num;
        }
    }
}
