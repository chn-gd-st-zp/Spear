using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog.Web;

namespace Spear.MidM.Logger
{
    public static class MidModule_Logger
    {
        public static IHostBuilder UseLogger(this IHostBuilder hostBuilder)
        {
            return hostBuilder
                .ConfigureLogging((hostingContext, config) =>
                {
                    config.ClearProviders();
                    //config.AddNLog("nlog.config");
                })
                .UseNLog();
        }
    }
}
