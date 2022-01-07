using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerUI;

using Spear.Inf.Core.Tool;

namespace Spear.MidM.Swagger
{
    public static class MidModule_Swagger
    {
        public static IServiceCollection AddSwagger(
            this IServiceCollection services, SwaggerSettings swaggerSettings, List<string> xmls,
            string tokenKey = null, OpenApiSecurityScheme oass = null, OpenApiSecurityRequirement oasr = null
        )
        {
            var oai = new OpenApiInfo
            {
                Title = swaggerSettings.Title,
                Version = swaggerSettings.Version,
            };

            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(swaggerSettings.Code, oai);

                options.EnableAnnotations();
                options.DocumentFilter<ApiHiddenFilter>();
                options.DocumentFilter<PropertyOperationFilter>();
                options.DocumentFilter<EnumDescriptionFilter>();
                options.OperationFilter<AccessTokenInHeaderFilter>();

                foreach (string xml in xmls)
                    options.IncludeXmlComments(xml, true);

                if (!tokenKey.IsEmptyString() && oass != null && oasr != null)
                {
                    options.AddSecurityDefinition(tokenKey, oass);
                    options.AddSecurityRequirement(oasr);
                }
            });
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, SwaggerSettings swaggerSettings)
        {
            string url = "/swagger/" + swaggerSettings.Code + "/swagger.json";

            return app
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.RoutePrefix = string.Empty;
                    options.SwaggerEndpoint(url, swaggerSettings.Title);

                    options.DefaultModelExpandDepth(2);
                    options.DefaultModelRendering(ModelRendering.Model);
                    options.DefaultModelsExpandDepth(-1);
                    options.DisplayOperationId();
                    options.DisplayRequestDuration();
                    options.DocExpansion(DocExpansion.None);
                    //options.EnableDeepLinking();
                    //options.EnableFilter();
                    options.ShowExtensions();
                });
        }
    }
}
