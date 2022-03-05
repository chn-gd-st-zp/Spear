using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Spear.Inf.Core.Tool;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.DTO;

namespace Spear.MidM.Swagger
{
    public class EnumDescriptionFilter : IDocumentFilter
    {
        public void Apply2(OpenApiDocument doc, DocumentFilterContext context)
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

                        if (!inputProperty.PropertyType.IsExtendOf(typeof(Enum)))
                            continue;

                        var prop = doc.Components.Schemas[inputProperty.PropertyType.Name];

                        prop.Enum = new List<IOpenApiAny>();
                        prop.Description = string.Empty;
                        foreach (var item in inputProperty.PropertyType.ToDictionary())
                        {
                            prop.Enum.Add(new OpenApiString(item.Value[0]));
                            prop.Description += $"<br>{item.Key}:{item.Value[0]}-{item.Value[1]}";
                        }

                        var attr = inputProperty.GetCustomAttribute<RemarkAttribute>();
                        if (attr != null)
                            prop.Description = attr.Remark + prop.Description;
                    }
                }
            }
        }

        public void Apply(OpenApiDocument doc, DocumentFilterContext context)
        {
            foreach (ApiDescription apiDescription in context.ApiDescriptions)
            {
                var method = default(MethodInfo);
                if (!apiDescription.TryGetMethodInfo(out method))
                    continue;

                var paramsList = new List<ParameterInfo>();

                //添加输入参数
                paramsList.AddRange(method.GetParameters());

                //添加输出参数
                paramsList.Add(method.ReturnParameter);

                foreach (var paramsItem in paramsList)
                {
                    PropertyOperation(doc, paramsItem.ParameterType);
                }
            }
        }

        private void PropertyOperation(OpenApiDocument doc, Type inputType)
        {
            if (inputType.IsGenericType)
            {
                foreach (var gType in inputType.GetGenericArguments())
                    PropertyOperation(doc, gType);
            }

            var inputTypeName = inputType.GetFullTypeName();
            if (!doc.Components.Schemas.ContainsKey(inputTypeName))
                return;

            var inputTypeSchema = doc.Components.Schemas[inputTypeName];

            //遍历字段
            foreach (var inputProperty in inputType.GetProperties())
            {
                if (inputProperty.PropertyType.IsGenericType)
                {
                    foreach (var gType in inputProperty.PropertyType.GetGenericArguments())
                        PropertyOperation(doc, gType);
                }
                else if (inputProperty.PropertyType.IsImplementedOf(typeof(IDTO)))
                {
                    PropertyOperation(doc, inputProperty.PropertyType);
                }
                else
                {
                    var swaggerProperty = inputTypeSchema.GetSwaggerProperty(inputProperty.Name);

                    //找不到字段信息
                    if (swaggerProperty == null)
                        continue;

                    if (!inputProperty.PropertyType.IsExtendOf(typeof(Enum)))
                        continue;

                    var prop = doc.Components.Schemas[inputProperty.PropertyType.Name];

                    prop.Enum = new List<IOpenApiAny>();
                    prop.Description = string.Empty;
                    foreach (var item in inputProperty.PropertyType.ToDictionary())
                    {
                        prop.Enum.Add(new OpenApiString(item.Value[0]));
                        prop.Description += $"<br>{item.Key}:{item.Value[0]}-{item.Value[1]}";
                    }

                    var attr = inputProperty.GetCustomAttribute<RemarkAttribute>();
                    if (attr != null)
                        prop.Description = attr.Remark + prop.Description;
                }
            }
        }
    }
}
