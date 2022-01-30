using System;
using System.Collections.Generic;

using Spear.Inf.Core.Attr;

namespace Spear.Inf.Core.Tool
{
    public static class EnumConverter
    {
        /// <summary>
        /// 将字符串转为枚举
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ToEnum(this Type type, string value)
        {
            try
            {
                return Enum.Parse(type, value, true);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 将数字转为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this int value) where T : Enum
        {
            try
            {
                return (T)Enum.ToObject(typeof(T), value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 将字符串转为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value) where T : Enum
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 将字符串转为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ToEnumList<T>() where T : Enum
        {
            List<T> result = new List<T>();

            try
            {
                string[] nameArray = Enum.GetNames(typeof(T));
                foreach (string name in nameArray)
                    result.Add(ToEnum<T>(name));
            }
            catch
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 将字符串转为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataArray"></param>
        /// <returns></returns>
        public static List<T> ToEnumList<T>(this string[] dataArray) where T : Enum
        {
            List<T> result = new List<T>();

            try
            {
                foreach (string data in dataArray)
                    result.Add(ToEnum<T>(data));
            }
            catch
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 将枚举转为字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string[]> ToDictionary(this Type type)
        {
            var dic = new Dictionary<int, string[]>();

            var eValueArray = Enum.GetValues(type);
            foreach (var eValue in eValueArray)
            {
                var key = (int)eValue;
                var value = new string[] { eValue.ToString(), type.GetRemark(eValue.ToString()) };

                dic.Add(key, value);
            }

            return dic;
        }

        /// <summary>
        /// 将枚举转为字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<int, string[]> ToDictionary<T>() where T : Enum
        {
            var dic = new Dictionary<int, string[]>();

            var eValueArray = Enum.GetValues(typeof(T));
            foreach (var eValue in eValueArray)
            {
                var key = (int)eValue;
                var value = new string[] { eValue.ToString(), eValue.ToString().ToEnum<T>().GetRemark() };

                dic.Add(key, value);
            }

            return dic;
        }
    }
}
