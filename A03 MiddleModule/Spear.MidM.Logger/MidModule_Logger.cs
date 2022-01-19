using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Autofac;
using NLog.Web;
using Serilog;

using Spear.Inf.Core.Interface;

namespace Spear.MidM.Logger
{
    public static class MidModule_Logger
    {
        public static IHostBuilder UseNLogger(this IHostBuilder hostBuilder)
        {
            return hostBuilder
                .ConfigureLogging((hostingContext, config) =>
                {
                    config.ClearProviders();
                    config.AddNLog("nlog.config");
                })
                .UseNLog();
        }

        public static IHostBuilder UseSeriLogger(this IHostBuilder hostBuilder)
        {
            return hostBuilder
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile($"serilog.json", true, false);
                })
                .ConfigureLogging((hostingContext, config) =>
                {
                    config.ClearProviders();
                })
                .UseSerilog();
        }

        public static ContainerBuilder RegisNLogger(this ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            containerBuilder.RegisterType<NLogger>().As<ISpearLogger>().SingleInstance();
            containerBuilder.RegisterGeneric(typeof(NLogger<>)).As(typeof(ISpearLogger<>)).SingleInstance();

            return containerBuilder;
        }

        public static ContainerBuilder RegisSeriLogger(this ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            containerBuilder.RegisterType<SeriLogger>().As<ISpearLogger>().SingleInstance();
            containerBuilder.RegisterGeneric(typeof(SeriLogger<>)).As(typeof(ISpearLogger<>)).SingleInstance();

            return containerBuilder;
        }
    }
}
