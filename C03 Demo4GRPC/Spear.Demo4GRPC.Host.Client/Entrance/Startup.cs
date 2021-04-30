using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.ServGeneric.MicServ;
using Spear.MidM.Logger;

using ServiceContext = Spear.Inf.Core.ServGeneric.ServiceContext;
using CUS = Spear.Inf.Core.Interface;

namespace Spear.Demo4GRPC.Host.Client
{
    public class Startup : StartupBasic<Settings>
    {
        public Startup(IConfiguration configuration) : base(configuration) { }

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
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services
                .AddControllers(options =>
                {
                    options.Filters.Add<CtrlerFilterAttribute>();
                })
                .AddControllersAsServices()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Formatting = JsonSerializerSettings.Formatting;
                    options.SerializerSettings.DateFormatString = JsonSerializerSettings.DateFormatString;
                    options.SerializerSettings.Converters = JsonSerializerSettings.Converters;
                });

            services.AddAutoMapper();
        }

        protected override void Extend_ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(o => CurConfig.MicServClientSettings).AsSelf().SingleInstance();

            containerBuilder.RegisterGeneric(typeof(NLogger<>)).As(typeof(CUS.ILogger<>)).InstancePerDependency();
        }

        protected override void Extend_Configure(IApplicationBuilder app, IHostEnvironment env, IHostApplicationLifetime lifetime, ILoggerFactory loggerFactory)
        {
            ServiceContext.InitServiceProvider(app.ApplicationServices);
            ServiceContext.InitMicServClient();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.UseMagicOnion();
            });
        }
    }
}
