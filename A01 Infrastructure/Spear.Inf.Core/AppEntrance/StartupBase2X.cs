using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace Spear.Inf.Core.AppEntrance
{
    public abstract class StartupBase2X<TSettings, TConfigureCollection> : StartupBase<TSettings, TConfigureCollection>
        where TSettings : AppSettingsBase
        where TConfigureCollection : IConfigureCollection
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