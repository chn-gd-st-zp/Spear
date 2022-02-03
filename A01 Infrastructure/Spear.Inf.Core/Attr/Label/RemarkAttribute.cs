using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Spear.Inf.Core.Attr
{
    public class RemarkAttribute : DescriptionAttribute
    {
        public RemarkAttribute(string remark) : base(remark)
        {
            Remark = remark;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public static class RemarkAttributeExtension
    {
        /// <summary>
        /// 获取枚举的备注信息
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public static string GetRemark(this Enum em)
        {
            return  em.GetType().GetRemark(em.ToString());
        }

        /// <summary>
        /// 获取枚举的备注信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetRemark(this Type type)
        {
            string result = string.Empty;

            var attr = type.GetCustomAttributes<RemarkAttribute>().FirstOrDefault();
            if (attr == null)
                return result;

            result = attr.Remark;

            return result;
        }

        /// <summary>
        /// 获取枚举的备注信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetRemark(this PropertyInfo property)
        {
            string result = string.Empty;

            var attr = property.GetCustomAttributes<RemarkAttribute>().FirstOrDefault();
            if (attr == null)
                return result;

            result = attr.Remark;

            return result;
        }

        /// <summary>
        /// 获取枚举的备注信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetRemark(this Type type, string value)
        {
            string result = string.Empty;

            FieldInfo fi = type.GetField(value);
            if (fi == null)
                return result;

            var attr = fi.GetCustomAttributes<RemarkAttribute>().FirstOrDefault();
            if (attr == null)
                return result;

            result = attr.Remark;

            return result;
        }
    }
}
