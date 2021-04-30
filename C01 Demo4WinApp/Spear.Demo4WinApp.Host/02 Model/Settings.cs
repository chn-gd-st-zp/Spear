using System.Collections.Generic;

using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.SettingsGeneric;
using Spear.MidM.Quartz;

namespace Spear.Demo4WinApp.Host
{
    public class Settings : AppSettingsBasic
    {
        public JobSettings JobSettings { get; set; }
    }

    [DIModeForSettings("AutoDelSettings", Enum_DIType.Specific, typeof(AutoDelSettings))]
    public class AutoDelSettings : List<AutoDelSettingsItem>, ISettings
    {
        //
    }

    public class AutoDelSettingsItem
    {
        public string ABSPath { get; set; }
        public string[] FileType { get; set; }
        public AutoDelExpired Expired { get; set; }
    }

    public class AutoDelExpired
    {
        public Enum_DateCycle EType { get; set; }
        public int NumericValue { get; set; }
    }
}
