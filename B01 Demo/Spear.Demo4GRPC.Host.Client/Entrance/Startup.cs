using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Tool;
using Spear.MidM.Logger;
using Spear.MidM.Swagger;
using Spear.MidM.MicoServ;
using Spear.MidM.MicoServ.MagicOnion;
using Spear.GlobalSupport.Base.Filter;

namespace Spear.Demo4GRPC.Host.Client
{
    public class Startup : StartupBase3X<Settings, AppConfiguresBase>
    {
        public Startup(IConfiguration configuration) : base(configuration) { }

        public void Configure(IApplicationBuilder app, IHostEnvironment env, IHostApplicationLifetime lifetime, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider apiVerDescProvider)
        {
            var configures = new AppConfiguresBase()
            {
                App = app,
                Env = env,
                Lifetime = lifetime,
                LoggerFactory = loggerFactory,
                ApiVerDescProvider = apiVerDescProvider,
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

            services.AddSwagger(CurConfig.SwaggerSettings);
            services.AddAutoMapper();
        }

        protected override void Extend_ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisStateCodeValueConverter<Enum_StateCode>();
            containerBuilder.RegisNLogger(Configuration);
            containerBuilder.Register(o => CurConfig.MicoServClientSettings).AsSelf().SingleInstance();
            containerBuilder.RegisMicoServProvider<MagicOnionProvider>();
        }

        protected override void Extend_Configure(AppConfiguresBase configures)
        {
            if (configures.Env.IsDevelopment())
            {
                configures.App.UseDeveloperExceptionPage();
            }

            configures.UseSwagger(CurConfig.SwaggerSettings);

            configures.App.UseRouting();
            configures.App.UseAuthorization();
            configures.App.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            configures.UseMicoServ();
        }
    }
}
