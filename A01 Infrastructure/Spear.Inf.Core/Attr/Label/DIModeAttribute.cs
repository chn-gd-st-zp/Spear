using System;
using System.Collections.Generic;
using System.Linq;

using Autofac.Core;

using Spear.Inf.Core.CusEnum;

namespace Spear.Inf.Core.Attr
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public abstract class DIModeAttribute : Attribute
    {
        public Enum_DIType EDIType { get; protected set; }

        public Type Type { get; protected set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DIModeForArrayItemAttribute : Attribute { }

    public class DIModeForServiceAttribute : DIModeAttribute
    {
        public object Key { get; private set; }

        public List<Parameter> Params { get; private set; }

        public DIModeForServiceAttribute()
        {
            EDIType = Enum_DIType.AsSelf;
        }

        public DIModeForServiceAttribute(Enum_DIType eDIType, Type type = null)
        {
            EDIType = eDIType;
            Type = type;
        }

        public DIModeForServiceAttribute(Enum_DIType eDIType, Type type, object key)
        {
            EDIType = eDIType;
            Type = type;
            Key = key;
        }

        public DIModeForServiceAttribute(Enum_DIType eDIType, Type type, object key, params Parameter[] paramArray)
        {
            EDIType = eDIType;
            Type = type;
            Key = key;
            Params = paramArray.ToList();
        }
    }

    public class DIModeForSettingsAttribute : DIModeAttribute
    {
        public string ConfigRootName { get; private set; }

        public Enum_DIKeyedNamedFrom KNFrom { get; private set; } = Enum_DIKeyedNamedFrom.None;

        public object Key { get; private set; }

        public DIModeForSettingsAttribute(string configRootName, Type type = null)
        {
            ConfigRootName = configRootName;
            Type = type;
            EDIType = Enum_DIType.Exclusive;
        }

        public DIModeForSettingsAttribute(string configRootName, Type type = null, Enum_DIKeyedNamedFrom knFrom = Enum_DIKeyedNamedFrom.FromProperty, object key = null)
        {
            ConfigRootName = configRootName;
            Type = type;
            EDIType = Enum_DIType.ExclusiveByKeyed;
            Key = key;
            KNFrom = knFrom;
        }
    }
}
