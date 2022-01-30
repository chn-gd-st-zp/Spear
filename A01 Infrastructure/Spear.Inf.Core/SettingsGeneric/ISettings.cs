using Spear.Inf.Core.Injection;

namespace Spear.Inf.Core.SettingsGeneric
{
    public interface ISettings : ISingleton
    {
        //
    }

    public class AppSettingsBase : ISettings
    {
        public InjectionSettings InjectionSettings { get; set; }
    }
}
