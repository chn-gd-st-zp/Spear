using System;
using System.Linq;

using Microsoft.Extensions.Configuration;

using Spear.Inf.Core.AppEntrance;

namespace Spear.Inf.Core.SettingsGeneric
{
    public static class SettingsGenericFactory
    {
        private static ISettingsGeneric _settingGeneric;
        private static ISettingsGeneric SettingGeneric
        {
            get
            {
                if (_settingGeneric == null)
                    _settingGeneric = new RegularSettingsGeneric();

                return _settingGeneric;
            }
        }

        public static bool ContainsNode(this IConfigurationRoot configRoot, string nodeName)
        {
            return configRoot.AsEnumerable().Where(o => o.Key == nodeName).Count() > 0;
        }

        public static IConfigurationBuilder SetGeneric(this IConfigurationBuilder configBuilder, ISettingsGeneric settingGeneric)
        {
            _settingGeneric = settingGeneric;

            return configBuilder;
        }

        public static string GetSetting(this IConfiguration config, string rootName)
        {
            return SettingGeneric.GetSetting(config, rootName);
        }

        public static T GetSetting<T>(this IConfiguration config, string rootName) where T : AppSettingsBasic
        {
            return SettingGeneric.GetSetting<T>(config, rootName);
        }

        public static T GetSetting<T>(this IConfiguration config) where T : AppSettingsBasic
        {
            return SettingGeneric.GetSetting<T>(config);
        }

        public static object GetSetting(this IConfiguration config, string rootName, Type type)
        {
            return SettingGeneric.GetSetting(config, rootName, type);
        }
    }
}
