using System.Collections.Generic;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.Inf.Core.ServGeneric.IOC
{
    [DIModeForSettings("AutoFacSettings", Enum_DIType.Specific, typeof(AutoFacSettings))]
    public class AutoFacSettings : ISettings
    {
        public string DefaultPattern { get; set; }

        public List<string> Dlls { get; set; }
    }
}
