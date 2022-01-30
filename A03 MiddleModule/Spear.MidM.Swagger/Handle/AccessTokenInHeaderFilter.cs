using System.Collections.Generic;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Spear.Inf.Core;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Swagger
{
    public class AccessTokenInHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var swaggerSettings = ServiceContext.Resolve<SwaggerSettings>();
            if (swaggerSettings == null)
                return;

            if(swaggerSettings.AccessTokenKeyInHeader.IsEmptyString())
                return;

            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(
                new OpenApiParameter
                {
                    Name = swaggerSettings.AccessTokenKeyInHeader,
                    In = ParameterLocation.Header,
                }
            );
        }
    }
}
