using System;

using Microsoft.Extensions.Configuration;

using Spear.Inf.Core.AppEntrance;

namespace Spear.Inf.Core.SettingsGeneric
{
    public interface ISettingsGeneric
    {
        string GetSetting(IConfiguration config, string rootName);

        T GetSetting<T>(IConfiguration config, string rootName) where T : AppSettingsBase;

        T GetSetting<T>(IConfiguration config) where T : AppSettingsBase;

        object GetSetting(IConfiguration config, string rootName, Type type);
    }
}
