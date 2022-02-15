using System;

namespace Spear.Inf.Core.Tool
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 是否已经过去
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsPast(this string date)
        {
            return date.ToDateTime().IsPast();
        }

        /// <summary>
        /// 是否已经过去
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsPast(this DateTime dateTime)
        {
            DateTime now = DateTime.Now;

            return now > dateTime;
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime Now(this DateTime dateTime)
        {
            return DateTime.Now.AddHours(AppInitHelper.TimeZone);
        }
    }
}
