using Spear.Inf.Core.Attr;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.MidM.MicoServ
{
    [DIModeForSettings("MicoServClientSettings", typeof(MicoServClientSettings))]
    public class MicoServClientSettings : ISettings
    {
        public Enum_AccessMode AccessMode { get; set; }

        public Enum_RegisCenter RegisCenter { get; set; }

        public string ServAddress { get; set; }
    }
}
