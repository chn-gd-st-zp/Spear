using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Autofac.Extensions.DependencyInjection;

using Spear.Inf.Core.AppEntrance;
using Spear.MidM.Logger;
using Spear.MidM.MicoServ;

namespace Spear.Demo4GRPC.Host.Client
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
                    config.LoadRunningSettings(args, MicoServExtend.LoadMicoServDeploySettings);
                })
                .UseNLogger()
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
