using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace Spear.Inf.Core.AppEntrance
{
    public abstract class StartupBase2X<TSettings, TConfigures> : StartupBase<TSettings, TConfigures>
        where TSettings : AppSettingsBase
        where TConfigures : AppConfiguresBase
    {
        public StartupBase2X(IConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// ≈‰÷√∑˛ŒÒ
        /// </summary>
        /// <param name="services"></param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            Extend_ConfigureServices(services);

            var containerBuilder = new ContainerBuilder();

            containerBuilder.Populate(services);

            ConfigureContainer(containerBuilder);

            return new AutofacServiceProvider(containerBuilder.Build());
        }
    }
}