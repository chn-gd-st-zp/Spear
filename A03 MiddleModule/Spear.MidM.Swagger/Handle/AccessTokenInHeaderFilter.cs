using System.Collections.Generic;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Spear.Inf.Core.ServGeneric;

namespace Spear.MidM.Swagger
{
    public class AccessTokenInHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = ServiceContext.Resolve<SwaggerSettings>().AccessTokenKeyInHeader,
                In = ParameterLocation.Header,
            });
        }
    }
}
