using System;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Spear.MidM.Swagger
{
    public class ApiHiddenFilter : IDocumentFilter
    {
        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="context"></param>

        public void Apply(OpenApiDocument doc, DocumentFilterContext context)
        {
            foreach (ApiDescription apiDescription in context.ApiDescriptions)
            {
                if (apiDescription.TryGetMethodInfo(out MethodInfo method))
                {
                    if (method.ReflectedType.CustomAttributes.Any(t => t.AttributeType == typeof(ApiHiddenAttribute))
                            || method.CustomAttributes.Any(t => t.AttributeType == typeof(ApiHiddenAttribute)))
                    {
                        string routeKey = "/" + apiDescription.RelativePath;
                        if (routeKey.Contains("?"))
                        {
                            int idx = routeKey.IndexOf("?", StringComparison.Ordinal);
                            routeKey = routeKey.Substring(0, idx);
                        }
                        doc.Paths.Remove(routeKey);
                    }
                }
            }
        }
    }
}
