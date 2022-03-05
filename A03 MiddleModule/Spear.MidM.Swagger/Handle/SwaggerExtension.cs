using System;
using System.Threading.Tasks;

using Microsoft.OpenApi.Models;

using Spear.Inf.Core.Tool;

namespace Spear.MidM.Swagger
{
    internal static class SwaggerExtension
    {
        internal static string GetFullTypeName(this Type type)
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

        internal static Tuple<string, OpenApiSchema> GetSwaggerProperty(this OpenApiSchema schema, string propertyName)
        {
            var inputPropertyKey = string.Empty;
            var inputPropertySchema = default(OpenApiSchema);

            //遍历所有字段名标识
            foreach (var key in schema.Properties.Keys)
            {
                //找出与字段匹配的字段名标识
                if (!propertyName.Equals(key, StringComparison.OrdinalIgnoreCase))
                    continue;

                inputPropertyKey = key;
                inputPropertySchema = schema.Properties[inputPropertyKey];

                //找到立刻跳出循环
                break;
            }

            //找不到字段信息
            if (inputPropertyKey.IsEmptyString() || inputPropertySchema == default)
                return null;

            return new Tuple<string, OpenApiSchema>(inputPropertyKey, inputPropertySchema);
        }
    }
}
