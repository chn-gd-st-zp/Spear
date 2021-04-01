using System;

namespace Spear.MidM.Swagger
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public partial class PropertyHiddenAttribute : Attribute { }
}
