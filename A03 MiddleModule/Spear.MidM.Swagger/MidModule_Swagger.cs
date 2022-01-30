using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.SwaggerGen;

using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Swagger
{
    public static class MidModule_Swagger
    {
        public static IServiceCollection AddSwagger(
            this IServiceCollection services, SwaggerSettings swaggerSettings,
            OpenApiSecurityScheme oass = null, OpenApiSecurityRequirement oasr = null
        )
        {
            services
                .AddApiVersioning(option =>
                {
                    // 可选，为true时API返回支持的版本信息
                    option.ReportApiVersions = true;

                    // 请求中未指定版本时默认为1.0
                    option.DefaultApiVersion = new ApiVersion(1, 0);

                    //版本号以什么形式，什么字段传递
                    option.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader(),
                        new QueryStringApiVersionReader(swaggerSettings.VersionKeyInQuery),
                        new HeaderApiVersionReader(swaggerSettings.VersionKeyInHeader)
                    );

                    // 在不提供版本号时，默认为1.0  如果不添加此配置，不提供版本号时会报错"message": "An API version is required, but was not specified."
                    //option.AssumeDefaultVersionWhenUnspecified = true;

                    //默认以当前最高版本进行访问
                    //option.ApiVersionSelector = new CurrentImplementationApiVersionSelector(option);
                })
                .AddVersionedApiExplorer(opt =>
                {
                    //以通知swagger替换控制器路由中的版本并配置api版本
                    opt.SubstituteApiVersionInUrl = true;

                    // 版本名的格式：v+版本号
                    opt.GroupNameFormat = "'v'VVV";

                    //是否提供API版本服务
                    opt.AssumeDefaultVersionWhenUnspecified = true;
                })
                .AddSwaggerGen()
                .AddOptions<SwaggerGenOptions>()
                .Configure<IApiVersionDescriptionProvider>((options, service) =>
                {
                    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    foreach (var description in service.ApiVersionDescriptions)
                    {
                        var oai = new OpenApiInfo()
                        {
                            //标题
                            Title = swaggerSettings.Title,

                            //当前版本
                            Version = description.ApiVersion.ToString(),

                            //文档说明
                            Description = description.IsDeprecated ? "此版本已放弃兼容" : "",

                            //TermsOfService = new Uri(""),

                            //联系方式
                            //Contact = new OpenApiContact() { Name = "", Email = "", Url = null },

                            //许可证
                            //License = new OpenApiLicense() { Name = "", Url = new Uri("") }
                        };

                        options.SwaggerDoc(description.GroupName, oai);
                    }

                    options.EnableAnnotations();
                    options.DocumentFilter<ApiHiddenFilter>();
                    options.DocumentFilter<PropertyOperationFilter>();
                    options.DocumentFilter<EnumDescriptionFilter>();
                    options.OperationFilter<AccessTokenInHeaderFilter>();

                    List<string> xmls = AppInitHelper.GetPaths(Enum_InitFile.XML, swaggerSettings.Patterns, swaggerSettings.Xmls);
                    foreach (string xml in xmls)
                        options.IncludeXmlComments(xml, true);

                    if (!swaggerSettings.AccessTokenKeyInHeader.IsEmptyString() && oass != null && oasr != null)
                    {
                        options.AddSecurityDefinition(swaggerSettings.AccessTokenKeyInHeader, oass);
                        options.AddSecurityRequirement(oasr);
                    }
                });

            return services;
        }

        public static void UseSwagger(this AppConfiguresBase appConfigures, SwaggerSettings swaggerSettings, Action<SwaggerUIOptions> optionsAction = null)
        {
            appConfigures.App
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    foreach (var description in appConfigures.ApiVerDescProvider.ApiVersionDescriptions)
                    {
                        options.RoutePrefix = swaggerSettings.RoutePrefix.ToLower();
                        options.SwaggerEndpoint($"{description.GroupName}/swagger.json", $"V{description.ApiVersion}");

                        if (optionsAction != null)
                            optionsAction(options);
                        else
                        {
                            options.DocExpansion(DocExpansion.None);
                            options.DisplayOperationId();
                            options.DisplayRequestDuration();
                            options.EnableFilter();
                            options.EnableDeepLinking();
                            options.ShowExtensions();
                            options.DefaultModelExpandDepth(2);
                            options.DefaultModelsExpandDepth(2);
                            options.DefaultModelRendering(ModelRendering.Model);
                        }
                    }
                });
        }
    }
}
