using System;

using Spear.Inf.Core.CusEnum;

namespace Spear.Inf.Core.Attr
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class KVResourceAttribute : Attribute
    {
        //
    }

    [AttributeUsage(AttributeTargets.Enum)]
    public class OperationTypeAttribute : Attribute
    {
        public Enum_OperationType EOperationType { get; private set; }

        public OperationTypeAttribute(Enum_OperationType eOperationType)
        {
            EOperationType = eOperationType;
        }
    }
}
