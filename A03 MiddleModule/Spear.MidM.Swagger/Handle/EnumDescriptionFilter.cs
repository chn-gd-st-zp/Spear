using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Spear.Inf.Core.Tool;
using Spear.Inf.Core.Attr;

namespace Spear.MidM.Swagger
{
    public class EnumDescriptionFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument doc, DocumentFilterContext context)
        {
            foreach (ApiDescription apiDescription in context.ApiDescriptions)
            {
                var method = default(MethodInfo);
                if (!apiDescription.TryGetMethodInfo(out method))
                    continue;

                string actionRoute = "/" + apiDescription.RelativePath;
                if (actionRoute.Contains("?"))
                    actionRoute = actionRoute.Substring(0, actionRoute.IndexOf("?", StringComparison.Ordinal));

                foreach (var input in method.GetParameters())
                {
                    if (!doc.Components.Schemas.ContainsKey(input.ParameterType.Name))
                        continue;

                    var inputSchema = doc.Components.Schemas[input.ParameterType.Name];
                    var inputProperties = input.ParameterType.GetProperties();

                    foreach (var inputProperty in inputProperties)
                    {
                        if (!doc.Components.Schemas.ContainsKey(inputProperty.PropertyType.Name))
                            continue;

                        if (!inputProperty.PropertyType.IsExtendType(typeof(Enum)))
                            continue;

                        var prop = doc.Components.Schemas[inputProperty.PropertyType.Name];

                        prop.Enum = new List<IOpenApiAny>();
                        foreach (var item in inputProperty.PropertyType.Convert2Dictionary())
                            prop.Enum.Add(new OpenApiString($"{item.Key}:{item.Value[0]}-{item.Value[1]}"));

                        var attr = inputProperty.GetCustomAttribute<RemarkAttribute>();
                        if (attr != null)
                            prop.Description = attr.Remark;
                    }
                }
            }
        }
    }
}
