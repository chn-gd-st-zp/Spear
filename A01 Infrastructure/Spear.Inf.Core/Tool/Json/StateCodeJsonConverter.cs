using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.Tool
{
    /// <summary>
    /// StateCodeJsonConverter
    /// </summary>
    public class StateCodeJsonConverter : CustomCreationConverter<IStateCode>
    {
        private ISpearEnumConverter<IStateCode> _converter;

        /// <summary>
        /// 构造函数
        /// </summary>
        public StateCodeJsonConverter() { _converter = ServiceContext.Resolve<ISpearEnumConverter<IStateCode>>(); }

        /// <summary>
        /// 是否开启自定义反序列化，值为true时，反序列化时会走ReadJson方法，值为false时，不走ReadJson方法，而是默认的反序列化
        /// </summary>
        public override bool CanRead => false;

        /// <summary>
        /// 是否开启自定义序列化，值为true时，序列化时会走WriteJson方法，值为false时，不走WriteJson方法，而是默认的序列化
        /// </summary>
        public override bool CanWrite => true;

        /// <summary>
        /// 是否允许覆盖
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType) => true;

        /// <summary>
        /// 重载创建方法
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override IStateCode Create(Type objectType) { return default(IStateCode); }

        /// <summary>
        /// 重载反序列化方法
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) { return _converter.Read(reader.Value); }

        /// <summary>
        /// 重载序列化方法
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) { writer.WriteValue(_converter.Write(value as SpearEnumItem)); }
    }
}
