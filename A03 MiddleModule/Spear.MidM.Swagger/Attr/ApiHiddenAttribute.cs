using System;

namespace Spear.MidM.Swagger
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public partial class ApiHiddenAttribute : Attribute { }
}
