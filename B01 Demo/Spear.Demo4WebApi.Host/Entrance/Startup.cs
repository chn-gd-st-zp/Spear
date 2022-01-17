using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;
using Spear.Inf.EF;
using Spear.MidM.Logger;
using Spear.MidM.Swagger;

using Spear.Demo.DBIns.Stainless;

using ServiceContext = Spear.Inf.Core.ServGeneric.ServiceContext;

namespace Spear.Demo4WebApi.Host
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

            services.AddSwagger(CurConfig.SwaggerSettings, AppInitHelper.GetPaths(Enum_InitFile.XML, CurConfig.SwaggerSettings.Patterns, CurConfig.SwaggerSettings.Xmls));
        }

        protected override void Extend_ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisSeriLogger(Configuration);

            var efOptionsBuilder = new EFDBContextOptionsBuilder<EFDBContext_Stainless>()
            {
                BulidAction = (optionsBuilder) =>
                {
                    optionsBuilder.UseSqlServer(ServiceContext.Resolve<DBConnectionSettings>().Stainless);

                    return optionsBuilder.Options;
                }
            };
            containerBuilder.Register(o => efOptionsBuilder).AsSelf().InstancePerDependency();

            containerBuilder.RegisterType<EFDBContext_Stainless>().As<IDBContext>().InstancePerDependency();
            containerBuilder.RegisterType<EFDBContext_Stainless>().As<EFDBContext_Stainless>().InstancePerDependency();
            containerBuilder.RegisterType<EFDBContext_Stainless>().Keyed<IDBContext>(Enum_ORMType.EF).InstancePerDependency();
            containerBuilder.RegisterType<EFDBContext_Stainless>().Keyed<EFDBContext_Stainless>(Enum_ORMType.EF).InstancePerDependency();
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
            });
        }
    }
}
