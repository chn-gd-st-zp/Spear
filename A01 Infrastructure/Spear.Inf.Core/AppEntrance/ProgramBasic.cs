using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Autofac;
using Autofac.Extensions.DependencyInjection;
//using Com.Ctrip.Framework.Apollo;

using Spear.Inf.Core.Tool;
using Spear.Inf.Core.SettingsGeneric;
using Spear.Inf.Core.CusEnum;

namespace Spear.Inf.Core.AppEntrance
{
    public static class ProgramBasic
    {
        /// <summary>
        /// ≥ı ºªØwin≥Ã–Ú
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <typeparam name="TConfig"></typeparam>
        /// <typeparam name="TSettingGeneric"></typeparam>
        /// <param name="configFiles"></param>
        public static void InitWinHost<TStartup, TConfig, TSettingGeneric>(params string[] configFiles)
            where TStartup : StartupBasic<TConfig>
            where TConfig : AppSettingsBasic
            where TSettingGeneric : ISettingsGeneric
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetGeneric(InstanceCreator.Create<TSettingGeneric>());
            builder.LoadConfiguration(configFiles);

            if (AppInitHelper.EEnvironment != Enum_Environment.Development)
            {
                //builder
                //.AddApollo(builder.Build().GetSection("apollo"))
                //.AddDefault()
                //.AddNamespace("Starup")
                //.AddNamespace("AutoFacSettings")
                //.AddNamespace("SwaggerSettings")
                //.AddNamespace("EncryptionNDecrypt")
                //.AddNamespace("ExteriorModule");
            }

            IConfiguration configuration = builder.Build();
            IServiceCollection services = new ServiceCollection();

            TStartup startup = (TStartup)InstanceCreator.Create(typeof(TStartup), configuration);
            startup.ConfigureServices(services);

            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            startup.ConfigureContainer(containerBuilder);

            var serviceProvider = new AutofacServiceProvider(containerBuilder.Build());
            startup.Extend_Configure(serviceProvider, null, null, null);
        }
    }
}
