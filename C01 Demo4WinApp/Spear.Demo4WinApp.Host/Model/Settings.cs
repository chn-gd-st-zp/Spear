﻿using System.Collections.Generic;

using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.SettingsGeneric;
using Spear.MidM.Quartz;

namespace Spear.Demo4WinApp.Host
{
    public class Settings : AppSettingsBase
    {
        public JobSettings JobSettings { get; set; }
    }

    [DIModeForSettings("AutoDelSettings", Enum_DIType.Exclusive, typeof(AutoDelSettings))]
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

    [DIModeForSettings("DBSettings", Enum_DIType.Exclusive, typeof(HAHAHAHA_DB))]
    public class HAHAHAHA_DB : ISettings
    {
        [Decrypt(Enum_EncryptionNDecrypt.AES)]
        public string Name { get; set; }
    }

    [DIModeForSettings("TestSettings", Enum_DIType.Exclusive, typeof(HAHAHAHA_List))]
    [DIModeForArrayItem]
    public class HAHAHAHA_List : List<HAHAHAHA_Item>, ISettings
    {
        //
    }

    [DIModeForSettings("TestSettings", Enum_DIType.ExclusiveByKeyed, typeof(HAHAHAHA_Item), "Name", Enum_DIKeyedNamedFrom.FromProperty)]
    public class HAHAHAHA_Item : ISettings
    {
        public string Name { get; set; }
    }
}