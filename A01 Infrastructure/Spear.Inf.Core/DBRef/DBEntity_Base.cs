using System.Linq;

using Spear.Inf.Core.Tool;

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
            var property = type.GetProperties().Where(o => o.Name.IsEqual(fieldName)).SingleOrDefault();
            if (property == null)
                return null;

            return property.GetValue(dbEntity);
        }
    }
}
