using System;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.Inf.Core.DBRef
{
    public class DBOptionFactory<TOption, TDBType, TConnectionSettings, TConnectionSettingsKey> : IDBOptionFactory<TOption, TDBType, TConnectionSettings, TConnectionSettingsKey>
        where TOption : class
        where TDBType : Enum
        where TConnectionSettings : ISettings
    {
        public TOption Option
        {
            get
            {
                return default(TOption);
            }
        }
    }
}
