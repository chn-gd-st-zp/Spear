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
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric.MicServ;
using Spear.Inf.Core.CusEnum;
using Spear.MidM.Logger;
using Spear.MidM.Swagger;

using ServiceContext = Spear.Inf.Core.ServGeneric.ServiceContext;

namespace Spear.Demo4GRPC.Host.Server
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

            services.AddSwagger(CurConfig.SwaggerSettings, AppInitHelper.GetPaths(Enum_InitFile.XML, CurConfig.SwaggerSettings.Patterns, CurConfig.SwaggerSettings.Xmls));
            services.AddAutoMapper();
        }

        protected override void Extend_ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(o => CurConfig.MicServRunSettings).AsSelf().SingleInstance();
            containerBuilder.Register(o => CurConfig.MicServServerSettings).AsSelf().SingleInstance();

            containerBuilder.RegisterGeneric(typeof(NLogger<>)).As(typeof(ISpearLogger<>)).InstancePerDependency();
        }

        protected override void Extend_Configure(AppConfiguresBase configures)
        {
            ServiceContext.InitServiceProvider(configures.App.ApplicationServices);

            if (configures.Env.IsDevelopment())
            {
                configures.App.UseDeveloperExceptionPage();
            }

            configures.App.UseSwagger(CurConfig.SwaggerSettings);

            configures.App.UseRouting();
            configures.App.UseAuthorization();
            configures.App.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.UseMagicOnion();
            });

            configures.App.UseConsul(configures.Lifetime, CurConfig.MicServRunSettings, CurConfig.MicServServerSettings);
        }
    }
}
