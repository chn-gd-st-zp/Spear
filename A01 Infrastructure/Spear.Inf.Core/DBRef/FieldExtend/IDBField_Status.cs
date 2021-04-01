using System.Linq;

using Spear.Inf.Core.CusEnum;

namespace Spear.Inf.Core.DBRef
{
    public interface IDBField_Status
    {
        string Status { get; set; }

        Enum_Status EStatus { get; set; }
    }

    public static class IDBField_Status_Ext
    {
        public static IQueryable<T> FilterDel<T>(this IQueryable<T> query) where T : IDBField_Status
        {
            query = query.Where(o =>
                o.Status != Enum_Status.Delete.ToString()
            );

            return query;
        }
    }
}
