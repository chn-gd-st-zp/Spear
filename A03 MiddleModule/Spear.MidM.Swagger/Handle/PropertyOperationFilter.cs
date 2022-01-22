using System;
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

                    PropertyOperation(doc, inputSchema, inputProperties);

                    #region 隐藏字段

                    foreach (var inputProperty in inputProperties)
                    {
                        var attr_hid = inputProperty.GetCustomAttribute<PropertyHiddenAttribute>();
                        if (attr_hid == null)
                            continue;

                        foreach (var inputPropertyKey in inputSchema.Properties.Keys)
                        {
                            if (!inputProperty.Name.Equals(inputPropertyKey, StringComparison.OrdinalIgnoreCase))
                                continue;
                            var inputPropertySchema = inputSchema.Properties[inputPropertyKey];

                            inputSchema.Properties.Remove(inputPropertyKey);
                        }
                    }

                    #endregion
                }
            }
        }

        private void PropertyOperation(OpenApiDocument doc, OpenApiSchema classSchema, PropertyInfo[] inputProperties)
        {
            //遍历字段
            foreach (var inputProperty in inputProperties)
            {
                //获取字段类型
                var propertyType = inputProperty.PropertyType;

                if (propertyType.IsClass && propertyType.IsExtendType(typeof(IDTO_Input)))
                {
                    //如果是继承了IDTO_Input的类

                    //从文档中判断是否存在该类型的结构说明
                    if (!doc.Components.Schemas.ContainsKey(propertyType.Name))
                        continue;

                    //获取该字段的结构说明
                    var classSchema_Property = doc.Components.Schemas[propertyType.Name];

                    //递归调用
                    PropertyOperation(doc, classSchema_Property, inputProperty.PropertyType.GetProperties());
                }
                else
                {
                    var inputPropertyKey = "";
                    var inputPropertySchema = default(OpenApiSchema);

                    //遍历所有字段名标识
                    foreach (var key in classSchema.Properties.Keys)
                    {
                        //找出与字段匹配的字段名标识
                        if (!inputProperty.Name.Equals(key, StringComparison.OrdinalIgnoreCase))
                            continue;

                        inputPropertyKey = key;
                        inputPropertySchema = classSchema.Properties[inputPropertyKey];

                        //找到立刻跳出循环
                        break;
                    }

                    //找不到字段信息
                    if (inputPropertyKey.IsEmptyString() || inputPropertySchema == default)
                        continue;

                    var attr_hid = inputProperty.GetCustomAttribute<PropertyHiddenAttribute>();
                    if (attr_hid != null)
                    {
                        classSchema.Properties.Remove(inputPropertyKey);
                    }

                    //如果不存在重命名的标签就进入下个匹配
                    var attr_ren = inputProperty.GetCustomAttribute<PropertyRenameAttribute>();
                    if (attr_ren != null)
                    {
                        classSchema.Properties.Remove(inputPropertyKey);
                        classSchema.Properties.Add(attr_ren.Name.FormatPropertyName(), inputPropertySchema);
                    }
                }
            }
        }
    }
}
