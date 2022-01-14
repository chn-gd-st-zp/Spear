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
using Spear.MidM.Logger;
using Spear.MidM.Schedule;

using ServiceContext = Spear.Inf.Core.ServGeneric.ServiceContext;
using Hangfire;

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
            services.Monitor<AutoDelSettings>(Configuration);

            services.RegisSchedule(this, CurConfig.ScheduleSettings);
            //services.RegisHangFire();
        }

        protected override void Extend_ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisSeriLogger(Configuration);

            containerBuilder.RegisQuartz(this);
        }

        protected override void Extend_Configure(AppConfiguresBase configures)
        {
            ServiceContext.InitServiceProvider(configures.App.ApplicationServices);

            configures.App.MonitorSettings(this.GetRunningType());

            //configures.App.UseHangfireDashboard("/hangfire");
        }
    }
}
