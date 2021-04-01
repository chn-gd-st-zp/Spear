using System;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Spear.MidM.Swagger
{
    public class PropertyHiddenFilter : IDocumentFilter
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
                    //[参数列表] 获取
                    var paramArray = method.GetParameters();

                    foreach (var param in paramArray)
                    {
                        //[参数对象的字段] 循环检查
                        foreach (var pi in param.ParameterType.GetProperties())
                        {
                            //[字段隐藏标签] 是否设置
                            if (pi.GetCustomAttribute<PropertyHiddenAttribute>() == null)
                                continue;

                            string routeKey = "/" + apiDescription.RelativePath;
                            if (routeKey.Contains("?"))
                            {
                                int idx = routeKey.IndexOf("?", StringComparison.Ordinal);
                                routeKey = routeKey.Substring(0, idx);
                            }

                            #region 删除 属性备注 形式1

                            foreach (var key in doc.Components.Schemas[param.ParameterType.Name].Properties.Keys)
                            {
                                if (pi.Name.Equals(key, StringComparison.OrdinalIgnoreCase))
                                {
                                    doc.Components.Schemas[param.ParameterType.Name].Properties.Remove(key);
                                    break;
                                }
                            }

                            #endregion

                            #region 删除 属性备注 形式2

                            foreach (var operation in doc.Paths[routeKey].Operations)
                            {
                                var obj = operation.Value.Parameters.Where(o => pi.Name.Equals(o.Name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                                operation.Value.Parameters.Remove(obj);
                            }

                            #endregion
                        }
                    }
                }
            }
        }
    }
}
