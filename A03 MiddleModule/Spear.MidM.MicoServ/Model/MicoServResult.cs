using System.Text.Json.Serialization;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.MicoServ
{
    public abstract class MicoServResult<TData>
    {
        public bool IsSuccess { get; set; }

        [JsonConverter(typeof(StateCodeJsonConverter<Enum_StateCode>))]
        public SpearEnumItem Code { get; set; }

        public string Msg { get; set; }

        public TData Data { get; set; }

        public string ErrorStackTrace { get; set; }
    }
}
