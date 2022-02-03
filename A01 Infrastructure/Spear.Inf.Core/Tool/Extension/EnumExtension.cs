using System;
using System.Reflection;

namespace Spear.Inf.Core.Tool
{
    public static class EnumExtension
    {
        public static TAttr GetEnumAttr<TAttr>(this Type type, string value)
            where TAttr : Attribute
        {
            foreach (var field in type.GetFields())
            {
                if (!field.Name.IsEqual(value))
                    continue;

                return field.GetCustomAttribute<TAttr>();
            }

            return null;
        }
    }
}
