using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Autofac.Extensions.DependencyInjection;

using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.ServGeneric.MicServ;
using Spear.MidM.Logger;

namespace Spear.Demo4WebApi.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            var host = hostBuilder.Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var result = Microsoft.Extensions.Hosting.Host
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.LoadConfiguration();
                    config.LoadRunningSettings(args, MicServExtend.LoadMicServRunSettings);
                })
                .UseLogger()
                .ConfigureWebHostDefaults(hostBuilder =>
                {
                    hostBuilder.UseKestrel();
                    hostBuilder.UseStartup<Startup>();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());

            return result;
        }
    }
}
