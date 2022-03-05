using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Swagger
{
    public class PropertyOperationFilter : IDocumentFilter
    {
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

                    #region 隐藏字段 特殊处理

                    //string actionRoute = "/" + apiDescription.RelativePath;
                    //if (actionRoute.Contains("?"))
                    //    actionRoute = actionRoute.Substring(0, actionRoute.IndexOf("?", StringComparison.Ordinal));

                    string propertyName = paramsItem.ParameterType.GetFullTypeName();
                    if (!doc.Components.Schemas.ContainsKey(propertyName))
                        continue;

                    var inputSchema = doc.Components.Schemas[propertyName];
                    var inputProperties = paramsItem.ParameterType.GetProperties();
                    foreach (var inputProperty in inputProperties)
                    {
                        var attr_hid = inputProperty.GetCustomAttribute<PropertyHiddenAttribute>();
                        if (attr_hid == null)
                            continue;

                        foreach (var inputPropertyKey in inputSchema.Properties.Keys)
                        {
                            if (!inputProperty.Name.Equals(inputPropertyKey, StringComparison.OrdinalIgnoreCase))
                                continue;

                            inputSchema.Properties.Remove(inputPropertyKey);
                        }
                    }

                    #endregion
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

                    var attr_hid = inputProperty.GetCustomAttribute<PropertyHiddenAttribute>();
                    if (attr_hid != null)
                    {
                        inputTypeSchema.Properties.Remove(swaggerProperty.Item1);
                        continue;
                    }

                    //如果不存在重命名的标签就进入下个匹配
                    var attr_ren = inputProperty.GetCustomAttribute<PropertyRenameAttribute>();
                    if (attr_ren != null && !inputTypeSchema.Properties.ContainsKey(attr_ren.Name.FormatPropertyName()))
                    {
                        inputTypeSchema.Properties.Remove(swaggerProperty.Item1);
                        inputTypeSchema.Properties.Add(attr_ren.Name.FormatPropertyName(), swaggerProperty.Item2);
                        continue;
                    }
                }
            }
        }
    }
}
