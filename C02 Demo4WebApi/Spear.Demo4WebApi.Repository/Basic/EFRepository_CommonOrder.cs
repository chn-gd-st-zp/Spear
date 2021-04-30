using System;
using System.Linq;

using Spear.Inf.Core.DTO;
using Spear.Inf.Core.ServGeneric.IOC;
using Spear.DBIns.Stainless;
using Spear.DBIns.Stainless.Entity;

namespace Spear.Demo4WebApi.Repository
{
    public interface EFRepository_CommonOrder : ITransient
    {
        ODTO_Page<object> Page(DateTime? beginTime, DateTime? endTime, IDTO_Page pageParam);
    }

    public abstract class EFRepository_CommonOrder<TEntity, TKey> : EFRepository_Stainless<TEntity, TKey>, EFRepository_CommonOrder
        where TEntity : CommonData, new()
    {

        private IQueryable<TEntity> GenericQuery(DateTime? beginTime, DateTime? endTime)
        {
            var query = DBContext.GetDBSet<TEntity>().AsQueryable();

            if (beginTime.HasValue && endTime.HasValue)
            {
                query = query.Where(o => beginTime.Value <= o.CreateTime && o.CreateTime <= endTime.Value);
            }
            else if (beginTime.HasValue)
            {
                query = query.Where(o => beginTime.Value <= o.CreateTime);
            }
            else if (endTime.HasValue)
            {
                query = query.Where(o => o.CreateTime <= endTime.Value);
            }

            return query;
        }

        public ODTO_Page<object> Page(DateTime? beginTime, DateTime? endTime, IDTO_Page pageParam)
        {
            var query = GenericQuery(beginTime, endTime);
            var pageResult = DBContext.PageByQueryable<TEntity>(query, pageParam).ToODTOPage(pageParam);
            var result = new ODTO_Page<object>
            {
                PageSize = pageResult.PageSize,
                PageIndex = pageResult.PageIndex,
                TotalQty_Page = pageResult.TotalQty_Page,
                TotalQty_Row = pageResult.TotalQty_Row,
                Data = pageResult.Data.Select(o => o as object).ToList()
            };
            return result;
        }
    }
}
