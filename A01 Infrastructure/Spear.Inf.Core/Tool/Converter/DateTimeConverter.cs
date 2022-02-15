using System;

namespace Spear.Inf.Core.Tool
{
    public static class DateTimeConverter
    {
        #region 时间戳

        private static int ShortTimeStampLength
        {
            get
            {
                return Math.Round((DateTime.Now - BaseTime).TotalSeconds).ToString().Length;
            }
        }

        private static int LongTimeStampLength
        {
            get
            {
                return Math.Round((DateTime.Now - BaseTime).TotalMilliseconds).ToString().Length;
            }
        }

        private static DateTime BaseTime
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToUtc();
            }
        }

        private static DateTime ToUtc(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Utc);
        }

        /// <summary>
        /// DateTime转换为时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="digit">位数，默认10</param>
        /// <returns></returns>
        public static long ToTimeStamp(this DateTime dateTime, int digit = 10)
        {
            var result = 0d;

            var dt = dateTime.ToUtc().AddHours(AppInitHelper.TimeZone) - BaseTime;

            if (digit == ShortTimeStampLength)
                result = dt.TotalSeconds;

            if (digit == LongTimeStampLength)
                result = dt.TotalMilliseconds;

            return (long)result;
        }

        /// <summary>
        /// 时间戳转换为DateTime
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTimeFromTimeStamp(this long timeStamp)
        {
            var result = BaseTime;

            if (timeStamp.ToString().Length == ShortTimeStampLength)
                result = result.AddSeconds(timeStamp);

            if (timeStamp.ToString().Length == LongTimeStampLength)
                result = result.AddMilliseconds(timeStamp);

            return result;
        }

        #endregion

        #region 格式化

        /// <summary>
        /// 转换日期时间
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string date)
        {
            return DateTime.Parse(date);
        }

        /// <summary>
        /// 转换日期
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 转换日期时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// 转化成起始时间
        /// </summary>
        /// <param name="date">yyyy-MM-dd</param>
        /// <returns></returns>
        public static DateTime ToBeginTime(this string date)
        {
            if (!date.Contains("-"))
            {
                date = date.Insert(4, "-");
                date = date.Insert(7, "-");
            }

            return DateTime.Parse(date + " 00:00:00.000");
        }

        /// <summary>
        /// 转化成起始时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ToBeginTime(this DateTime dateTime)
        {
            return ToBeginTime(dateTime.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// 转化成结束时间
        /// </summary>
        /// <param name="date">yyyy-MM-dd</param>
        /// <returns></returns>
        public static DateTime ToEndTime(this string date)
        {
            if (!date.Contains("-"))
            {
                date = date.Insert(4, "-");
                date = date.Insert(7, "-");
            }

            return DateTime.Parse(date + " 23:59:59.999");
        }

        /// <summary>
        /// 转化成结束时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ToEndTime(this DateTime dateTime)
        {
            return ToEndTime(dateTime.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// 获取指定日期月份的最后一日
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this string month)
        {
            DateTime dt = DateTime.Parse(DateTime.Now.ToString("yyyy-") + month.PadLeft(2, '0') + DateTime.Now.ToString("-01 00:00:00"));
            return dt.FirstDayOfMonth();
        }

        /// <summary>
        /// 获取指定日期月份的第一日
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return DateTime.Parse(dateTime.ToString("yyyy-MM") + "-01 00:00:00");
        }

        /// <summary>
        /// 获取指定日期月份的最后一日
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this string month)
        {
            DateTime dt = DateTime.Parse(DateTime.Now.ToString("yyyy-") + month.PadLeft(2, '0') + DateTime.Now.ToString("-01 00:00:00"));
            return dt.LastDayOfMonth();
        }

        /// <summary>
        /// 获取指定日期月份的最后一日
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime dateTime)
        {
            return DateTime.Parse(DateTime.Parse(dateTime.AddMonths(1).ToString("yyyy-MM") + "-01 00:00:00").AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59");
        }

        /// <summary>
        /// 获取指定日期的下个月的第一日
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfNextMonth(this DateTime dateTime)
        {
            return DateTime.Parse(dateTime.AddMonths(1).ToString("yyyy-MM") + "-01 00:00:00");
        }

        /// <summary>
        /// 星期一
        /// </summary>
        public static DateTime Monday
        {
            get
            {
                var now = DateTimeOffset.Now;
                var i = now.DayOfWeek - DayOfWeek.Monday == -1 ? 6 : -1;
                var ts = new TimeSpan(i, 0, 0, 0);

                return now.Subtract(ts).Date;
            }
        }

        /// <summary>
        /// 星期二
        /// </summary>
        public static DateTime Tuesday => Monday.AddDays(1);

        /// <summary>
        /// 星期三
        /// </summary>
        public static DateTime Wednesday => Monday.AddDays(2);

        /// <summary>
        /// 星期四
        /// </summary>
        public static DateTime Thursday => Monday.AddDays(3);

        /// <summary>
        /// 星期五
        /// </summary>
        public static DateTime Friday => Monday.AddDays(4);

        /// <summary>
        /// 星期六
        /// </summary>
        public static DateTime Saturday => Monday.AddDays(5);

        /// <summary>
        /// 星期天
        /// </summary>
        public static DateTime Sunday => Monday.AddDays(6);

        #endregion
    }
}
