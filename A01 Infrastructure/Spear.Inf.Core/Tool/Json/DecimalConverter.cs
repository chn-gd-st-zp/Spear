using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Spear.Inf.Core.Tool
{
    /// <summary>
    /// 自定义数值类型序列化转换器(默认保留3位)
    /// </summary>
    public class DecimalConverter<TType> : CustomCreationConverter<TType>
    {
        /// <summary>
        /// 重载是否可写
        /// </summary>
        public override bool CanWrite { get { return true; } }

        /// <summary>
        /// 序列化后保留小数位数
        /// </summary>
        public int Digits { get; private set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="digits">序列化后保留小数位数</param>
        public DecimalConverter(int digits) { Digits = digits; }

        /// <summary>
        /// 重载创建方法
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override TType Create(Type objectType)
        {
            return default(TType);
        }

        /// <summary>
        /// 重载序列化方法
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var formatter = ((decimal)value).ToString("N" + Digits.ToString());
            writer.WriteValue(formatter);
        }
    }
}
