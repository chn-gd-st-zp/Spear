using System;

namespace Spear.MidM.Permission
{
    public class PermissionMappingType
    {
        public Type InputType { get; private set; }

        public Type DBDestinationType { get; private set; }

        public PermissionMappingType(Type inputType, Type dbDestinationType)
        {
            InputType = inputType;
            DBDestinationType = dbDestinationType;
        }
    }
}
