using SqlSugar;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.DBRef;

namespace Spear.DataBase.SqlSugar
{
    public static class SSExtend
    {
        public static ISugarQueryable<T> FilterDel<T>(this ISugarQueryable<T> query) where T : IDBField_Status
        {
            query = query.Where(o => o.Status != Enum_Status.Delete.ToString());

            return query;
        }
    }
}
