using Spear.Inf.Core.Attr;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.MidM.MicoServ
{
    [DIModeForSettings("MicoServDeploySettings", typeof(MicoServDeploySettings))]
    public class MicoServDeploySettings : ISettings
    {
        public string HostPublic { get; set; }

        public string HostInternal { get; set; }

        public int WebApiPort { get; set; }

        public int GRPCPort { get; set; }
    }
}
