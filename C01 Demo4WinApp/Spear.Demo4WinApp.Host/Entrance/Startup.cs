using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Spear.Inf.Core;
using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.SettingsGeneric;
using Spear.MidM.Defend;
using Spear.MidM.Logger;
using Spear.MidM.Quartz;

using ServiceContext = Spear.Inf.Core.ServGeneric.ServiceContext;

namespace Spear.Demo4WinApp.Host
{
    public class Startup : StartupBase3X<Settings, AppConfiguresBase>
    {
        public Startup(IConfiguration configuration) : base(configuration) { }

        public void Configure(IApplicationBuilder app, IHostEnvironment env, IHostApplicationLifetime lifetime, ILoggerFactory loggerFactory)
        {
            var configures = new AppConfiguresBase()
            {
                App = app,
                Env = env,
                Lifetime = lifetime,
                LoggerFactory = loggerFactory
            };

            Configure(configures);
        }

        protected override JsonSerializerSettings SetJsonSerializerSettings()
        {
            return new JsonSerializerSettings()
            {
                Formatting = Formatting.None,
                DateFormatString = "yyyy-MM-dd HH:mm:ss.fff",
                Converters = new List<JsonConverter> { new StringEnumConverter() },
            };
        }

        protected override void Extend_ConfigureServices(IServiceCollection services)
        {
            //services.AddHostedService<HostedService>();
            services.AddHostedService<HSSpear<QuartzRunner>>();

            services.Monitor<AutoDelSettings>(Configuration);
        }

        protected override void Extend_ConfigureContainer(ContainerBuilder containerBuilder)
        {
            //containerBuilder.RegisterType<QuartzRunner>().As<IRunner>().SingleInstance();
            containerBuilder.RegisterType<QuartzRunner>().AsSelf().SingleInstance();

            containerBuilder.RegisterGeneric(typeof(NLogger<>)).As(typeof(ISpearLogger<>)).InstancePerDependency();

            containerBuilder.RegisQuartz(this.GetRunningType(), CurConfig.JobSettings);

            containerBuilder.RegisAES("123456");
        }

        protected override void Extend_Configure(AppConfiguresBase configures)
        {
            ServiceContext.InitServiceProvider(configures.App.ApplicationServices);

            configures.App.MonitorSettings(this.GetRunningType());
        }
    }
}
