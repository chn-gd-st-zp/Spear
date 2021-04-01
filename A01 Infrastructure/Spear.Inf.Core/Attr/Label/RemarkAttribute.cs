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

    [AttributeUsage(AttributeTargets.Field)]
    public class RemarkOutputIgnoreAttribute : Attribute
    {
        //
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
            string result = string.Empty;

            Type type = em.GetType();

            FieldInfo fi = type.GetField(em.ToString());
            if (fi == null)
                return result;

            var attr = fi.GetCustomAttributes<RemarkAttribute>().FirstOrDefault();
            if (attr == null)
                return result;

            var attr_ignore = fi.GetCustomAttributes<RemarkOutputIgnoreAttribute>().FirstOrDefault();
            if (attr_ignore != null)
                return result;

            result = attr.Remark;

            return result;
        }
    }
}
