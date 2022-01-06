using System;
using System.Linq;
using System.Reflection;

using Spear.Inf.Core.DTO;

namespace Spear.Inf.Core.Attr
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DefaultSortFieldAttribute : Attribute
    {
        public DefaultSortFieldAttribute(string realName, Enum_SortDirection eDirection)
        {
            RealName = realName;
            EDirection = eDirection;
        }

        public string RealName { get; private set; }

        public Enum_SortDirection EDirection { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SortFieldAttribute : Attribute
    {
        public SortFieldAttribute(string realName, params string[] nickNames)
        {
            RealName = realName;
            NickNames = nickNames;
        }

        public string RealName { get; private set; }

        public string[] NickNames { get; private set; }
    }

    public static class SortFieldAttributeExtend
    {
        public static DefaultSortFieldAttribute GetDefaultSortField<T>(this Type type)
        {
            var attr = type.GetCustomAttribute<DefaultSortFieldAttribute>();
            if (attr != null)
                return attr;

            return null;
        }

        public static Tuple<PropertyInfo, string> GetSortField<T>(this object obj, IDTO_Sort sortField)
        {
            Type type = typeof(T);
            PropertyInfo[] piArray = type.GetProperties();

            return piArray.GetSortField<T>(sortField);
        }

        public static Tuple<PropertyInfo, string> GetSortField<T>(this PropertyInfo[] piArray, IDTO_Sort sortField)
        {
            foreach (var pi in piArray)
            {
                if (pi.Name.Equals(sortField.FieldName, StringComparison.OrdinalIgnoreCase))
                    return new Tuple<PropertyInfo, string>(pi, pi.Name);

                var attr = pi.GetCustomAttribute<SortFieldAttribute>();
                if (attr == null)
                    continue;

                if (attr.RealName.Equals(sortField.FieldName, StringComparison.OrdinalIgnoreCase))
                    return new Tuple<PropertyInfo, string>(pi, pi.Name);

                if (attr.NickNames.Contains(sortField.FieldName, StringComparer.OrdinalIgnoreCase))
                    return new Tuple<PropertyInfo, string>(pi, pi.Name);
            }

            return null;
        }
    }
}
