using System;

namespace Spear.Inf.Core.Tool
{
    public static class DateTimeConvert
    {
        #region 时间戳

        /// <summary>
        /// 时间戳计时开始时间
        /// </summary>
        private static DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static DateTimeOffset dto = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        /// <summary>
        /// DateTime转换为10位时间戳（单位：秒）
        /// </summary>
        /// <param name="dateTime"> DateTime</param>
        /// <returns>10位时间戳（单位：秒）</returns>
        public static long ToTimeStamp(this DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - dt).TotalSeconds;
        }

        /// <summary>
        /// DateTime转换为13位时间戳（单位：毫秒）
        /// </summary>
        /// <param name="dateTime"> DateTime</param>
        /// <returns>13位时间戳（单位：毫秒）</returns>
        public static long ToLongTimeStamp(this DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - dt).TotalMilliseconds;
        }

        /// <summary>
        /// 10位时间戳（单位：秒）转换为DateTime
        /// </summary>
        /// <param name="timeStamp">10位时间戳（单位：秒）</param>
        /// <returns>DateTime</returns>
        public static DateTime TimeStampToDateTime(this long timeStamp)
        {
            return dt.AddSeconds(timeStamp).ToLocalTime();
        }

        /// <summary>
        /// 13位时间戳（单位：毫秒）转换为DateTime
        /// </summary>
        /// <param name="longTimeStamp">13位时间戳（单位：毫秒）</param>
        /// <returns>DateTime</returns>
        public static DateTime LongTimeStampToDateTime(this long longTimeStamp)
        {
            return dt.AddMilliseconds(longTimeStamp).ToLocalTime();
        }

        /// <summary>
        /// 把Unix时间转换成DateTime
        /// </summary>
        /// <param name="unixTime">Unix时间</param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTime(long unixTime)
        {
            return dto.AddTicks(unixTime * 10).DateTime;
        }

        /// <summary>
        /// 获取当前Unix时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToUnixTimeString(this DateTime dateTime)
        {
            return ToUnixTimeNumber(dateTime).ToString("f0");
        }

        /// <summary>
        /// 获取当前Unix时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static double ToUnixTimeNumber(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().Subtract(dt).TotalMilliseconds;
        }

        #endregion

        #region 格式化

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// 是否已经过去
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsPass(this string date)
        {
            DateTime now = DateTime.Now;
            DateTime dataTime = date.ToEndTime();

            return now > dataTime;
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
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ToEndTime(this DateTime dateTime)
        {
            return ToEndTime(dateTime.ToString("yyyy-MM-dd"));
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
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this string month)
        {
            DateTime dt = DateTime.Parse(DateTime.Now.ToString("yyyy-") + month.PadLeft(2, '0') + DateTime.Now.ToString("-01 00:00:00"));
            return dt.FirstDayOfMonth();
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
        /// 获取指定日期月份的最后一日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this string month)
        {
            DateTime dt = DateTime.Parse(DateTime.Now.ToString("yyyy-") + month.PadLeft(2, '0') + DateTime.Now.ToString("-01 00:00:00"));
            return dt.LastDayOfMonth();
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
