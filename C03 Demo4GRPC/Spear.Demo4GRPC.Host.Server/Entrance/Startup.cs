using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
using Spear.Inf.Core.CusEnum;
using Spear.MidM.Logger;
using Spear.MidM.Swagger;

using ServiceContext = Spear.Inf.Core.ServGeneric.ServiceContext;
using CUS = Spear.Inf.Core.Interface;

namespace Spear.Demo4GRPC.Host.Server
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
                .Configure<ApiBehaviorOptions>(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                })
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

            services.AddGrpc();
            services.AddMagicOnion(o =>
            {
                o.EnableCurrentContext = true;
                o.GlobalFilters.AddFilter<GRPCFilterAttribute>();
            });

            services.AddSwagger(CurConfig.SwaggerSettings, CurConfig.SwaggerSettings.Xmls.GetPaths(Enum_InitFile.XML, CurConfig.SwaggerSettings.DefaultPattern));
            services.AddAutoMapper();
        }

        protected override void Extend_ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(o => CurConfig.MicServRunSettings).AsSelf().SingleInstance();
            containerBuilder.Register(o => CurConfig.MicServServerSettings).AsSelf().SingleInstance();

            containerBuilder.RegisterGeneric(typeof(NLogger<>)).As(typeof(CUS.ILogger<>)).InstancePerDependency();
        }

        protected override void Extend_Configure(IApplicationBuilder app, IHostEnvironment env, IHostApplicationLifetime lifetime, ILoggerFactory loggerFactory)
        {
            ServiceContext.InitServiceProvider(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(CurConfig.SwaggerSettings);

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.UseMagicOnion();
            });

            app.UseConsul(lifetime, CurConfig.MicServRunSettings, CurConfig.MicServServerSettings);
        }
    }
}
