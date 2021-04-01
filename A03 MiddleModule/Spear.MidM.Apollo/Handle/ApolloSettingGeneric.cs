using System;
using System.Reflection;

using Microsoft.Extensions.Configuration;

using Spear.Inf.Core.SettingsGeneric;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Apollo
{
    public class ApolloSettingGeneric : RegularSettingsGeneric
    {
        public override T GetSetting<T>(IConfiguration config)
        {
            T result = default;

            result = base.GetSetting<T>(config);
            if (result != null)
            {
                PropertyInfo[] piArray = typeof(T).GetProperties();
                foreach (var pi in piArray)
                {
                    string data = config[pi.Name];
                    try
                    {
                        if (!data.IsEmptyString())
                            pi.SetValue(result, data.ToObject(pi.PropertyType));
                    }
                    catch (Exception ex)
                    {
                        Printor.PrintText($"配置项有误：[{pi.Name}]");
                        Printor.PrintLine();
                        throw ex;
                    }
                }
            }

            return result;
        }

        public override object GetSetting(IConfiguration config, string rootName, Type type)
        {
            object result = null;

            result = base.GetSetting(config, rootName, type);
            if (result == null)
            {
                string json = base.GetSetting(config, rootName);
                result = json.ToObject(type);
            }

            return result;
        }
    }
}
