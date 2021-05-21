using System.Linq;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.DBRef;

namespace Spear.DataBase.EF
{
    public static class EFExtend
    {
        public static IQueryable<T> FilterDel<T>(this IQueryable<T> query) where T : IDBField_Status
        {
            query = query.Where(o => o.Status != Enum_Status.Delete.ToString());

            return query;
        }
    }
}
