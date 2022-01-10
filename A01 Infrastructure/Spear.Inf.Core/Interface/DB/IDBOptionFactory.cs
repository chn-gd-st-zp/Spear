using System;

using Spear.Inf.Core.SettingsGeneric;

namespace Spear.Inf.Core.Interface
{
    public interface IDBOptionFactory<TOption, TDBType, TConnectionSettings, TConnectionSettingsKey>
        where TOption : class
        where TDBType : Enum
        where TConnectionSettings : ISettings

    {
        public TOption Option { get; }
    }
}
