using Spear.Inf.Core.Attr;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.Inf.Core.ServGeneric.IOC
{
    [DIModeForSettings("InjectionSettings", typeof(InjectionSettings))]
    public class InjectionSettings : ISettings
    {
        public string[] Patterns { get; set; }

        public string[] Dlls { get; set; }
    }
}
