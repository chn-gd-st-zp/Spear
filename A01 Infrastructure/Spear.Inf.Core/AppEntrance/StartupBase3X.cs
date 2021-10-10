using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Spear.Inf.Core.AppEntrance
{
    public abstract class StartupBase3X<TSettings, TConfigureCollection> : StartupBase<TSettings, TConfigureCollection>
        where TSettings : AppSettingsBase
        where TConfigureCollection : IConfigureCollection
    {
        public StartupBase3X(IConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// ≈‰÷√∑˛ŒÒ
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            Extend_ConfigureServices(services);
        }
    }
}