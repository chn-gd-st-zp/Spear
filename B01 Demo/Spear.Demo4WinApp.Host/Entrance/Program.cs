using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Autofac.Extensions.DependencyInjection;
using Com.Ctrip.Framework.Apollo;

using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.SettingsGeneric;
using Spear.MidM.Apollo;
using Spear.MidM.Logger;
using Spear.MidM.MicoServ;

namespace Spear.Demo4WinApp.Host
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

                    //var configRoot = config.Build();
                    //if (configRoot.ContainsNode("ApolloSettings"))
                    //{
                    //    config.SetGeneric(new ApolloSettingsGeneric());
                    //    config
                    //    .AddApollo(configRoot.GetSection("ApolloSettings"))
                    //    .AddDefault()
                    //    .AddNamespace("???");
                    //}

                    config.LoadRunningSettings(args, MicoServExtend.LoadMicoServDeploySettings);
                })
                .UseNLogger()
                .UseSeriLogger()
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
