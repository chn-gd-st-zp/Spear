using System.Linq;

namespace Spear.Inf.Core.DBRef
{
    public abstract class DBEntity_Base
    {
        //
    }

    public static class DBEntityExtend
    {
        public static object GetFieldValue(this DBEntity_Base dbEntity, string fieldName)
        {
            if (dbEntity == null)
                return null;

            var type = dbEntity.GetType();
            var property = type.GetProperties().Where(o => string.Compare(o.Name, fieldName, true) == 0).SingleOrDefault();
            if (property == null)
                return null;

            return property.GetValue(dbEntity);
        }
    }
}
