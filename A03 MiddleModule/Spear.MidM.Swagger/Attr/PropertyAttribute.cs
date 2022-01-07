using System;

namespace Spear.MidM.Swagger
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public partial class PropertyHiddenAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public partial class PropertyRenameAttribute : Attribute 
    { 
        public string Name { get; set; }

        public PropertyRenameAttribute(string name) { Name = name; }
    }
}
