using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.Tool
{
    public class StateCodeJsonConverter<TStateCode> : CustomCreationConverter<TStateCode>
        where TStateCode : IStateCode, new()
    {
        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType) => false;

        public override TStateCode Create(Type objectType) { return default(TStateCode); }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var result = default(object);

            if (value.GetType() == typeof(SpearEnumItem))
            {
                var converter = ServiceContext.Resolve<ISpearEnumConverter<TStateCode>>();
                result = converter.Write(value as SpearEnumItem);
            }

            if (result != null)
                writer.WriteValue(result);
        }
    }
}
