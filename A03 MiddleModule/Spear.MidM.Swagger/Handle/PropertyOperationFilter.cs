using System;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Spear.MidM.Swagger
{
    public class PropertyOperationFilter : IDocumentFilter
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
                        var attr_hid = inputProperty.GetCustomAttribute<PropertyHiddenAttribute>();
                        var attr_ren = inputProperty.GetCustomAttribute<PropertyRenameAttribute>();

                        #region 形式1

                        foreach (var inputPropertyKey in inputSchema.Properties.Keys)
                        {
                            if (!inputProperty.Name.Equals(inputPropertyKey, StringComparison.OrdinalIgnoreCase))
                                continue;

                            var inputPropertySchema = inputSchema.Properties[inputPropertyKey];

                            if (attr_hid != null)
                                inputSchema.Properties.Remove(inputPropertyKey);

                            if (attr_ren != null)
                            {
                                inputSchema.Properties.Remove(inputPropertyKey);
                                inputSchema.Properties.Add(attr_ren.Name, inputPropertySchema);
                            }

                            break;
                        }

                        #endregion

                        #region 形式2

                        foreach (var actionOperation in doc.Paths[actionRoute].Operations)
                        {
                            var actionOperationParam = actionOperation.Value.Parameters.Where(o => inputProperty.Name.Equals(o.Name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                            if (actionOperationParam == null)
                                continue;

                            if (attr_hid != null)
                                actionOperation.Value.Parameters.Remove(actionOperationParam);

                            if (attr_ren != null)
                                actionOperationParam.Name = attr_ren.Name;
                        }

                        #endregion

                        Rename(inputSchema, inputProperty);
                    }
                }
            }
        }

        public void Rename(OpenApiSchema inputSchema, PropertyInfo inputProperty)
        {
            var propertyType = inputProperty.PropertyType;

            if (!propertyType.IsClass)
                return;

            foreach (var property in propertyType.GetProperties())
            {
                if (propertyType.IsClass)
                {
                    Rename(inputSchema, inputProperty);
                    continue;
                }

                var attr_ren = property.GetCustomAttribute<PropertyRenameAttribute>();
                if (attr_ren == null)
                    continue;

                foreach (var inputPropertyKey in inputSchema.Properties.Keys)
                {
                    if (!inputProperty.Name.Equals(inputPropertyKey, StringComparison.OrdinalIgnoreCase))
                        continue;

                    var inputPropertySchema = inputSchema.Properties[inputPropertyKey];
                    inputSchema.Properties.Add(attr_ren.Name, inputPropertySchema);
                }
            }
        }
    }
}
