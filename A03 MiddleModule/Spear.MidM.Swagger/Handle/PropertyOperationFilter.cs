using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

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

                var paramsList = new List<ParameterInfo>();

                //添加输入参数
                paramsList.AddRange(method.GetParameters());

                //添加输出参数
                paramsList.Add(method.ReturnParameter);

                foreach (var paramsItem in paramsList)
                {
                    string propertyName = GetFullPropertyName(paramsItem.ParameterType);
                    if (!doc.Components.Schemas.ContainsKey(propertyName))
                        continue;

                    var inputSchema = doc.Components.Schemas[propertyName];
                    var inputProperties = paramsItem.ParameterType.GetProperties();

                    PropertyOperation(doc, inputSchema, inputProperties);

                    #region 隐藏字段 特殊处理

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

        private string GetFullPropertyName(Type type)
        {
            var name = type.IsExtendOf<Task>() ? "" : type.Name;

            if (type.IsGenericType)
            {
                name = name.IndexOf("`") != -1 ? name.Remove(name.IndexOf("`")) : name;

                var name_tmp = "";
                foreach (var gType in type.GetGenericArguments())
                    name_tmp += GetFullPropertyName(gType);

                name = name_tmp + name;
            }

            return name;
        }

        private void PropertyOperation(OpenApiDocument doc, OpenApiSchema classSchema, PropertyInfo[] inputProperties)
        {
            //遍历字段
            foreach (var inputProperty in inputProperties)
            {
                //获取字段类型
                var propertyType = inputProperty.PropertyType;

                if (propertyType.IsGenericType)
                {
                    foreach (var gType in propertyType.GetGenericArguments())
                    {
                        //从文档中判断是否存在该类型的结构说明
                        if (!doc.Components.Schemas.ContainsKey(gType.Name))
                            continue;

                        //获取该字段的结构说明
                        var classSchema_Property = doc.Components.Schemas[gType.Name];

                        //递归调用
                        PropertyOperation(doc, classSchema_Property, gType.GetProperties());
                    }
                }
                else if (propertyType.IsClass && propertyType.IsExtendOf(typeof(IDTO_Input)))
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
                    var inputPropertyKey = string.Empty;
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
                        continue;
                    }

                    //如果不存在重命名的标签就进入下个匹配
                    var attr_ren = inputProperty.GetCustomAttribute<PropertyRenameAttribute>();
                    if (attr_ren != null)
                    {
                        classSchema.Properties.Remove(inputPropertyKey);
                        classSchema.Properties.Add(attr_ren.Name.FormatPropertyName(), inputPropertySchema);
                        continue;
                    }
                }
            }
        }
    }
}
