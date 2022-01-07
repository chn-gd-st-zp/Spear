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
                if (apiDescription.TryGetMethodInfo(out MethodInfo method))
                {
                    foreach (var param in method.GetParameters())
                    {
                        //[参数对象的字段] 循环检查
                        foreach (var pi in param.ParameterType.GetProperties())
                        {
                            //[字段标签隐藏]
                            var attr_hid = pi.GetCustomAttribute<PropertyHiddenAttribute>();
                            if (attr_hid != null)
                            {
                                #region 形式1

                                foreach (var key in doc.Components.Schemas[param.ParameterType.Name].Properties.Keys)
                                {
                                    if (pi.Name.Equals(key, StringComparison.OrdinalIgnoreCase))
                                    {
                                        doc.Components.Schemas[param.ParameterType.Name].Properties.Remove(key);
                                        break;
                                    }
                                }

                                #endregion

                                #region 形式2

                                string routeKey = "/" + apiDescription.RelativePath;
                                if (routeKey.Contains("?"))
                                {
                                    int idx = routeKey.IndexOf("?", StringComparison.Ordinal);
                                    routeKey = routeKey.Substring(0, idx);
                                }

                                foreach (var operation in doc.Paths[routeKey].Operations)
                                {
                                    var obj = operation.Value.Parameters.Where(o => pi.Name.Equals(o.Name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                                    if (obj != null)
                                        operation.Value.Parameters.Remove(obj);
                                }

                                #endregion
                            }

                            //[字段标签重命名]
                            var attr_ren = pi.GetCustomAttribute<PropertyRenameAttribute>();
                            if (attr_ren != null)
                            {
                                #region 形式1

                                foreach (var prop in doc.Components.Schemas[param.ParameterType.Name].Properties)
                                {
                                    if (pi.Name.Equals(prop.Key, StringComparison.OrdinalIgnoreCase))
                                    {
                                        doc.Components.Schemas[param.ParameterType.Name].Properties.Remove(prop.Key);
                                        doc.Components.Schemas[param.ParameterType.Name].Properties.Add(attr_ren.Name, prop.Value);

                                        break;
                                    }
                                }

                                #endregion

                                #region 形式2

                                string routeKey = "/" + apiDescription.RelativePath;
                                if (routeKey.Contains("?"))
                                {
                                    int idx = routeKey.IndexOf("?", StringComparison.Ordinal);
                                    routeKey = routeKey.Substring(0, idx);
                                }

                                foreach (var operation in doc.Paths[routeKey].Operations)
                                {
                                    var obj = operation.Value.Parameters.Where(o => pi.Name.Equals(o.Name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                                    if (obj != null)
                                        obj.Name = attr_ren.Name;
                                }

                                #endregion
                            }
                        }
                    }
                }
            }
        }
    }
}
