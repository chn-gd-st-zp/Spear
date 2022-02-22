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

                    string propertyName = GetFullTypeName(paramsItem.ParameterType);
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

        private string GetFullTypeName(Type type)
        {
            var name = type.IsExtendOf<Task>() ? "" : type.Name;

            if (type.IsGenericType)
            {
                name = name.IndexOf("`") != -1 ? name.Remove(name.IndexOf("`")) : name;

                var name_tmp = "";
                foreach (var gType in type.GetGenericArguments())
                    name_tmp += GetFullTypeName(gType);

                name = name_tmp + name;
            }

            return name;
        }

        private void PropertyOperation(OpenApiDocument doc, Type inputType)
        {
            if (inputType.IsGenericType)
            {
                foreach (var gType in inputType.GetGenericArguments())
                    PropertyOperation(doc, gType);
            }

            if (inputType.Name == "ODTO_TransactionMyRecord")
            {
                string a = "";
            }

            var inputTypeName = GetFullTypeName(inputType);
            if (!doc.Components.Schemas.ContainsKey(inputTypeName))
                return;

            var inputTypeSchema = doc.Components.Schemas[inputTypeName];

            //遍历字段
            foreach (var inputProperty in inputType.GetProperties())
            {
                if (inputProperty.PropertyType.IsImplementedOf(typeof(IDTO)))
                {
                    PropertyOperation(doc, inputProperty.PropertyType);
                }
                else
                {
                    var inputPropertyKey = string.Empty;
                    var inputPropertySchema = default(OpenApiSchema);

                    //遍历所有字段名标识
                    foreach (var key in inputTypeSchema.Properties.Keys)
                    {
                        //找出与字段匹配的字段名标识
                        if (!inputProperty.Name.Equals(key, StringComparison.OrdinalIgnoreCase))
                            continue;

                        inputPropertyKey = key;
                        inputPropertySchema = inputTypeSchema.Properties[inputPropertyKey];

                        //找到立刻跳出循环
                        break;
                    }

                    //找不到字段信息
                    if (inputPropertyKey.IsEmptyString() || inputPropertySchema == default)
                        continue;

                    var attr_hid = inputProperty.GetCustomAttribute<PropertyHiddenAttribute>();
                    if (attr_hid != null)
                    {
                        inputTypeSchema.Properties.Remove(inputPropertyKey);
                        continue;
                    }

                    //如果不存在重命名的标签就进入下个匹配
                    var attr_ren = inputProperty.GetCustomAttribute<PropertyRenameAttribute>();
                    if (attr_ren != null && !inputTypeSchema.Properties.ContainsKey(attr_ren.Name.FormatPropertyName()))
                    {
                        inputTypeSchema.Properties.Remove(inputPropertyKey);
                        inputTypeSchema.Properties.Add(attr_ren.Name.FormatPropertyName(), inputPropertySchema);
                        continue;
                    }
                }
            }
        }
    }
}
