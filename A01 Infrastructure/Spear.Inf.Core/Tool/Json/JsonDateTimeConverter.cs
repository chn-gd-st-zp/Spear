using Newtonsoft.Json.Converters;

namespace Spear.Inf.Core.Tool
{
    public class JsonDateTimeConverter : IsoDateTimeConverter
    {
        public JsonDateTimeConverter(string format) { DateTimeFormat = format; }
    }
}
