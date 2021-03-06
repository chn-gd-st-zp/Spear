using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Autofac;
using Hangfire;
using Hangfire.MemoryStorage;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.Tool;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.MidM.Schedule
{
    public static class MidModule_Schedule
    {
        public static IServiceCollection RegisSchedule<TSettings, TConfigures>(this IServiceCollection services, StartupBase<TSettings, TConfigures> startup, ScheduleSettings settings)
            where TSettings : AppSettingsBase
            where TConfigures : AppConfiguresBase
        {
            if (settings.RunningServices == null || settings.RunningServices.Count() == 0)
                return services;

            var hsType = typeof(IHostedService);

            AppInitHelper
                .GetAllType(startup.CurConfig.InjectionSettings.Patterns, startup.CurConfig.InjectionSettings.Dlls)
                .Where(o => o.IsClass && o.IsImplementedOf(hsType))
                .Select(o => o.GetCustomAttribute<DIModeForServiceAttribute>())
                .Where(o => o != null)
                .ToList()
                .ForEach(o =>
                {
                    if (settings.RunningServices.Contains(o.Key.ToString().ToEnum<Enum_ScheduleType>()))
                        services.AddSingleton(hsType, o.Type);
                });

            return services;
        }

        public static IServiceCollection RegisHangFire(this IServiceCollection services)
        {
            services
                .AddHangfire(configuration =>
                {
                    configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                    configuration.UseSimpleAssemblyNameTypeSerializer();
                    configuration.UseRecommendedSerializerSettings();
                    configuration.UseSerilogLogProvider();
                    configuration.UseMemoryStorage();
                    configuration.UseFilter(new AutomaticRetryAttribute { Attempts = 0 });
                })
                .AddHangfireServer();

            services.AddTransient<IRegister4Timer, Register4HangFire>();

            return services;
        }

        public static ContainerBuilder RegisQuartz<TSettings, TConfigures>(this ContainerBuilder containerBuilder, StartupBase<TSettings, TConfigures> startup)
            where TSettings : AppSettingsBase
            where TConfigures : AppConfiguresBase
        {
            containerBuilder.RegisterType<StdSchedulerFactory>().As<ISchedulerFactory>().SingleInstance();
            containerBuilder.RegisterType<JobFactory>().As<IJobFactory>().SingleInstance();

            AppInitHelper
                .GetAllType(startup.CurConfig.InjectionSettings.Patterns, startup.CurConfig.InjectionSettings.Dlls)
                .Where(o => o.IsClass && o.IsImplementedOf<IRunner4Timer>() && o.IsImplementedOf<IJob>())
                .ToList()
                .ForEach(o =>
                {
                    containerBuilder.RegisterType(o).Keyed<IJob>(o).InstancePerDependency();
                });

            containerBuilder.RegisterType<Register4Quartz>().As<IRegister4Timer>().InstancePerDependency();

            return containerBuilder;
        }
    }
}
