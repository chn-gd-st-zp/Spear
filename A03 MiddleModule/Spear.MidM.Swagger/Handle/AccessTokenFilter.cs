using System.Collections.Generic;
using System.Reflection;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Spear.Inf.Core;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Swagger
{
    public class AccessTokenFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var swaggerSettings = ServiceContext.Resolve<SwaggerSettings>();
            if (swaggerSettings == null)
                return;

            if (swaggerSettings.AccessTokenKeyInHeader.IsEmptyString())
                return;

            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor == null)
                return;

            var attr = descriptor.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>();
            if (attr != null)
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
