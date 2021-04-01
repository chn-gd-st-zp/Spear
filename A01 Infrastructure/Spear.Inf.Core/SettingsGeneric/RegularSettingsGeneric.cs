using System;

using Microsoft.Extensions.Configuration;

using Spear.Inf.Core.AppEntrance;

namespace Spear.Inf.Core.SettingsGeneric
{
    public class RegularSettingsGeneric : ISettingsGeneric
    {
        public virtual string GetSetting(IConfiguration config, string rootName)
        {
            return config[rootName];
        }

        public virtual T GetSetting<T>(IConfiguration config, string rootName) where T : AppSettingsBasic
        {
            return config.GetSection(rootName).Get<T>();
        }

        public virtual T GetSetting<T>(IConfiguration config) where T : AppSettingsBasic
        {
            return config.Get<T>();
        }

        public virtual object GetSetting(IConfiguration config, string rootName, Type type)
        {
            return config.GetSection(rootName).Get(type);
        }
    }
}
