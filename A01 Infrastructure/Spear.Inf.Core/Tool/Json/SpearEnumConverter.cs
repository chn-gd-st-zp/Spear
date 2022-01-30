using System;

using Newtonsoft.Json;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.Tool
{
    /// <summary>
    /// StateCodeJsonConverter
    /// </summary>
    public class SpearEnumConverter<TSpearEnum> : JsonConverter
        where TSpearEnum : ISpearEnum, new()
    {
        /// <summary>
        /// 是否开启自定义反序列化，值为true时，反序列化时会走ReadJson方法，值为false时，不走ReadJson方法，而是默认的反序列化
        /// </summary>
        public override bool CanRead => true;

        /// <summary>
        /// 是否开启自定义序列化，值为true时，序列化时会走WriteJson方法，值为false时，不走WriteJson方法，而是默认的序列化
        /// </summary>
        public override bool CanWrite => true;

        /// <summary>
        /// 是否允许覆盖
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType) => false;

        /// <summary>
        /// 重载反序列化方法
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = default(SpearEnumItem);

            if (reader.ValueType == typeof(SpearEnumItem))
            {
                var converter = ServiceContext.Resolve<ISpearEnumConverter<TSpearEnum>>();
                result = converter.Read(reader.Value);
            }

            return result;
        }

        /// <summary>
        /// 重载序列化方法
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var result = default(object);

            if (value.GetType() == typeof(SpearEnumItem))
            {
                var converter = ServiceContext.Resolve<ISpearEnumConverter<TSpearEnum>>();
                result = converter.Write(value as SpearEnumItem);
            }

            if (result != null)
                writer.WriteValue(result);
        }
    }
}
