using System.Collections.Generic;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.Inf.Core.ServGeneric.IOC
{
    [DIModeForSettings("AutoFacSettings", typeof(AutoFacSettings))]
    public class AutoFacSettings : ISettings
    {
        public string DefaultPattern { get; set; }

        public List<string> Dlls { get; set; }
    }
}
