using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Autofac;
using NLog.Web;
using Serilog;

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
                    //config.AddNLog("nlog.config");
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

        public static ContainerBuilder RegisGlobalSeriLogger(this ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            return containerBuilder;
        }
    }
}
