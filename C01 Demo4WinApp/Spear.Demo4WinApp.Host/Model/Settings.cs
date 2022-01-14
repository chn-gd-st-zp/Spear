using System.Collections.Generic;

using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.SettingsGeneric;
using Spear.MidM.Schedule;

namespace Spear.Demo4WinApp.Host
{
    public class Settings : AppSettingsBase
    {
        public ScheduleSettings ScheduleSettings { get; set; }
    }

    [DIModeForArrayItem]
    [DIModeForSettings("AutoDelSettings", typeof(AutoDelSettings))]
    public class AutoDelSettings : List<AutoDelSettingsItem>, ISettings
    {
        //
    }

    [DIModeForSettings("AutoDelSettingsItem", typeof(AutoDelSettingsItem), Enum_DIKeyedNamedFrom.FromProperty, "Name")]
    public class AutoDelSettingsItem : ISettings
    {
        public string Name { get; set; }
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
