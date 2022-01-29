using Newtonsoft.Json.Converters;

namespace Spear.Inf.Core.Tool
{
    public class DateTimeJsonConverter : IsoDateTimeConverter
    {
        public DateTimeJsonConverter(string format) { DateTimeFormat = format; }
    }
}
