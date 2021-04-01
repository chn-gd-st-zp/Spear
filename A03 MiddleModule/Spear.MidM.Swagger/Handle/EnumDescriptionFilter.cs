using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Spear.Inf.Core.Tool;

namespace Spear.MidM.Swagger
{
    public class EnumDescriptionFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var item in swaggerDoc.Components.Schemas)
            {
                Type type = Type.GetType(item.Key, false, true);
                if (type == null)
                    continue;

                if (!type.IsExtendType<Enum>())
                    continue;

                var propertyEnums = item.Value.Enum;
                if (propertyEnums != null && propertyEnums.Count > 0)
                {
                    List<OpenApiInteger> list = propertyEnums.Select(o => (OpenApiInteger)o).ToList();

                    item.Value.Description += DescribeEnum(type, list);
                }
            }
        }

        private static string DescribeEnum(Type type, List<OpenApiInteger> enums)
        {
            var enumDescriptions = new List<string>();

            foreach (var item in enums)
            {
                if (type == null) type = item.GetType();

                var value = Enum.Parse(type, item.Value.ToString());
                var desc = GetDescription(type, value);

                if (desc.IsEmptyString())
                    enumDescriptions.Add($"{item.Value.ToString()}:{Enum.GetName(type, value)}; ");
                else
                    enumDescriptions.Add($"{item.Value.ToString()}:{Enum.GetName(type, value)},{desc}; ");

            }

            return $"<br/>{Environment.NewLine}{string.Join("<br/>" + Environment.NewLine, enumDescriptions)}";
        }

        private static string GetDescription(Type t, object value)
        {
            foreach (MemberInfo mInfo in t.GetMembers())
            {
                if (mInfo.Name == t.GetEnumName(value))
                {
                    foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo))
                    {
                        if (attr.IsExtendType<DescriptionAttribute>())
                        {
                            return ((DescriptionAttribute)attr).Description;
                        }
                    }
                }
            }

            return string.Empty;
        }
    }
}
