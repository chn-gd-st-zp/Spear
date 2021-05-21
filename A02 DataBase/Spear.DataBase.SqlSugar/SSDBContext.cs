using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

using SqlSugar;

using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.SqlSugar
{
    public abstract class SSDBContext : SqlSugarClient, IDBContext
    {
        public string ID { get { return _id; } }
        private string _id = Unique.GetGUID();

        public SSDBContext(ConnectionConfig connectionConfig) : base(connectionConfig) { }

        public SSDBContext(List<ConnectionConfig> connectionConfigList) : base(connectionConfigList) { }

        #region 增

        public bool Create<TEntity>(TEntity obj, bool save = true) where TEntity : DBEntity_Basic, new()
        {
            return GetSimpleClient<TEntity>().Insert(obj);
        }

        public bool Create<TEntity>(IEnumerable<TEntity> objs, bool save = true) where TEntity : DBEntity_Basic, new()
        {
            return GetSimpleClient<TEntity>().InsertRange(objs.ToList());
        }

        #endregion

        #region 删

        public bool Delete<TEntity, TKey>(TKey key) where TEntity : DBEntity_Basic, IDBField_ID<TKey>, new()
        {
            return GetSimpleClient<TEntity>().DeleteById(key);
        }

        public bool Delete<TEntity>(TEntity obj, bool save = true) where TEntity : DBEntity_Basic, new()
        {
            return GetSimpleClient<TEntity>().Delete(obj);
        }

        public bool Delete<TEntity>(IEnumerable<TEntity> objs, bool save = true) where TEntity : DBEntity_Basic, new()
        {
            return Deleteable(objs.ToList()).ExecuteCommand() > 0;
        }

        public bool Delete<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : DBEntity_Basic, new()
        {
            return GetSimpleClient<TEntity>().Delete(expression);
        }

        #endregion

        #region 改

        public bool Update<TEntity>(TEntity obj, bool save = true) where TEntity : DBEntity_Basic, new()
        {
            return GetSimpleClient<TEntity>().Update(obj);
        }

        public bool Update<TEntity>(IEnumerable<TEntity> objs, bool save = true) where TEntity : DBEntity_Basic, new()
        {
            return GetSimpleClient<TEntity>().UpdateRange(objs.ToList());
        }

        #endregion

        #region 查 - 单个

        public TEntity Single<TEntity, TKey>(TKey key) where TEntity : DBEntity_Basic, IDBField_ID<TKey>, new()
        {
            return GetSimpleClient<TEntity>().GetById(key);
        }

        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : DBEntity_Basic, new()
        {
            return GetSimpleClient<TEntity>().GetSingle(expression);
        }

        #endregion

        #region 查 - 列表

        public List<TEntity> List<TEntity, TKey>(params TKey[] keys) where TEntity : DBEntity_Basic, IDBField_ID<TKey>, new()
        {
            var query = GetSimpleClient<TEntity>().AsQueryable();

            if (keys != null && keys.Length != 0)
                query = query.Where(o => keys.Contains(o.ID));

            return query.ToList();
        }

        public List<TEntity> List<TEntity>(Expression<Func<TEntity, bool>> expression = null, IDTO_List param = null) where TEntity : DBEntity_Basic, new()
        {
            var query = GetSimpleClient<TEntity>().AsQueryable();

            if (expression != null)
                query = query.Where(expression);

            if (param != null)
                query = query.OrderBy(param.Sort.GenerOrderBySql<TEntity>());

            return query.ToList();
        }

        public List<TEntity> ListByQueryable<TEntity>(object queryObj, IDTO_List param = null) where TEntity : DBEntity_Basic, new()
        {
            string orderBy = param.Sort.GenerOrderBySql<TEntity>();

            return ((ISugarQueryable<TEntity>)queryObj).OrderBy(orderBy).ToList();
        }

        #endregion

        #region 查 - 分页

        public Tuple<List<TEntity>, int> Page<TEntity>(Expression<Func<TEntity, bool>> expression, IDTO_Page param = null) where TEntity : DBEntity_Basic, new()
        {
            var query = GetSimpleClient<TEntity>().AsQueryable().Where(expression);

            return PageByQueryable<TEntity>(query, param);
        }

        public Tuple<List<TEntity>, int> PageByQueryable<TEntity>(object queryObj, IDTO_Page param = null) where TEntity : DBEntity_Basic, new()
        {
            string orderBy = param.Sort.GenerOrderBySql<TEntity>();

            var query = ((ISugarQueryable<TEntity>)queryObj).OrderBy(orderBy);

            int rowQty = query.Count();

            param.PageSize = param.PageSize == 0 ? 10 : param.PageSize;
            param.PageIndex = param.PageIndex == 0 ? 1 : param.PageIndex;

            var dataList = query.ToPageList(param.PageIndex, param.PageSize, ref rowQty);

            return new Tuple<List<TEntity>, int>(dataList, rowQty);
        }

        #endregion

        #region 执行 SQL

        public int ExecuteSql(string sql, params DBParameter[] paramArray)
        {
            return Ado.ExecuteCommand(sql, paramArray.Parse());
        }

        public List<TEntity> SelectFromSql<TEntity>(string sql, params DBParameter[] paramArray) where TEntity : DBEntity_Basic, new()
        {
            return Ado.SqlQuery<TEntity>(sql, paramArray.Parse());
        }

        public int ExecuteStoredProcedure(string sql, params DBParameter[] paramArray)
        {
            return Ado.UseStoredProcedure().ExecuteCommand(sql, paramArray.Parse());
        }

        public List<TEntity> SelectFromStoredProcedure<TEntity>(string sql, params DBParameter[] paramArray) where TEntity : DBEntity_Basic, new()
        {
            DataTable dt = Ado.UseStoredProcedure().GetDataTable(sql, paramArray.Parse());

            return dt.ToDataList<TEntity>();
        }

        #endregion
    }

    public static class DBContextExtend
    {
        public static List<SugarParameter> Parse(this DBParameter[] paramArray)
        {
            if (paramArray == null)
                return new List<SugarParameter>();

            return paramArray
                .Select(o => new SugarParameter(o.Name, o.Value)
                {
                    IsNullable = o.IsNullable,
                    Direction = o.Direction,
                    DbType = o.DBType,
                    Size = o.Size,
                })
                .ToList();
        }
    }
}
