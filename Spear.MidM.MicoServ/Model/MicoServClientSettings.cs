using Spear.Inf.Core.Attr;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.MidM.MicoServ
{
    [DIModeForSettings("MicoServClientSettings", typeof(MicoServClientSettings))]
    public class MicoServClientSettings : ISettings
    {
        public Enum_RegisCenter MSType { get; set; }

        public Enum_AccessMode ReqMode { get; set; }

        public string Address { get; set; }
    }
}
