using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core.DBRef
{
    public class DBRepository : IDBRepository
    {
        public virtual IDBContext DBContext { get; protected set; }

        public DBRepository() { DBContext = GetDBContext(); }

        protected virtual IDBContext GetDBContext() { return ServiceContext.Resolve<IDBContext>(); }

        public virtual TQueryable DataFilter<TQueryable, TEntityWithStatus>(TQueryable queryObj)
            where TQueryable : class
            where TEntityWithStatus : IDBField_Status
        { return queryObj; }

        #region 查 - 列表

        public virtual List<T> ListByQueryable<T>(object queryObj, IDTO_List param = null) where T : DBEntity_Base, new()
        {
            return DBContext.ListByQueryable<T>(queryObj, param);
        }

        #endregion

        #region 查 - 分页

        public virtual Tuple<List<T>, int> PageByQueryable<T>(object queryObj, IDTO_Page param = null) where T : DBEntity_Base, new()
        {
            return DBContext.PageByQueryable<T>(queryObj, param);
        }

        #endregion

        #region 执行 SQL

        public virtual int ExecuteSql(string sql, params DBParameter[] paramArray)
        {
            return DBContext.ExecuteSql(sql, paramArray);
        }

        public virtual List<T> SelectFromSql<T>(string sql, params DBParameter[] paramArray) where T : DBEntity_Base, new()
        {
            return DBContext.SelectFromSql<T>(sql, paramArray);
        }

        public virtual int ExecuteStoredProcedure(string sql, params DBParameter[] paramArray)
        {
            return DBContext.ExecuteStoredProcedure(sql, paramArray);
        }

        public virtual List<T> SelectFromStoredProcedure<T>(string sql, params DBParameter[] paramArray) where T : DBEntity_Base, new()
        {
            return DBContext.SelectFromStoredProcedure<T>(sql, paramArray);
        }

        #endregion
    }

    public class DBRepository<TEntity> : DBRepository, IDBRepository<TEntity>
        where TEntity : DBEntity_Base, new()
    {
        #region 增

        public virtual bool Create(TEntity obj)
        {
            return DBContext.Create(obj);
        }

        public virtual bool Create(IEnumerable<TEntity> objs)
        {
            return DBContext.Create(objs);
        }

        #endregion

        #region 删

        public virtual bool Delete(TEntity obj)
        {
            return DBContext.Delete(obj);
        }

        public virtual bool Delete(IEnumerable<TEntity> objs)
        {
            return DBContext.Delete(objs);
        }

        public virtual bool Delete(Expression<Func<TEntity, bool>> expression)
        {
            return DBContext.Delete(expression);
        }

        #endregion

        #region 改

        public virtual bool Update(TEntity obj)
        {
            return DBContext.Update(obj);
        }

        public virtual bool Update(IEnumerable<TEntity> objs)
        {
            return DBContext.Update(objs);
        }

        #endregion

        #region 查 - 单个

        public virtual TEntity Single(Expression<Func<TEntity, bool>> expression)
        {
            return DBContext.Single(expression);
        }

        #endregion

        #region 查 - 列表

        public virtual List<TEntity> List(IDTO_List param = null)
        {
            return DBContext.List<TEntity>(null, param);
        }

        public virtual List<TEntity> List(Expression<Func<TEntity, bool>> expression, IDTO_List param = null)
        {
            return DBContext.List(expression, param);
        }

        #endregion

        #region 查 - 分页

        public virtual Tuple<List<TEntity>, int> Page(IDTO_Page param = null)
        {
            return DBContext.Page<TEntity>(null, param);
        }

        public virtual Tuple<List<TEntity>, int> Page(Expression<Func<TEntity, bool>> expression, IDTO_Page param = null)
        {
            return DBContext.Page(expression, param);
        }

        #endregion
    }

    public class DBRepository<TEntity, TKey> : DBRepository<TEntity>, IDBRepository<TEntity, TKey>
        where TEntity : DBEntity_Base, IDBField_ID<TKey>, new()
    {
        #region 删

        public virtual bool Delete(TKey key)
        {
            return DBContext.Delete<TEntity, TKey>(key);
        }

        #endregion

        #region 查 - 单个

        public virtual TEntity Single(TKey key)
        {
            return DBContext.Single<TEntity, TKey>(key);
        }

        #endregion

        #region 查 - 列表

        public virtual List<TEntity> List(params TKey[] keys)
        {
            return DBContext.List<TEntity, TKey>(keys);
        }

        #endregion
    }

    public class DBRepository<TDBContext, TEntity, TKey> : DBRepository<TEntity, TKey>, IDBRepository<TDBContext, TEntity, TKey>
        where TDBContext : IDBContext
        where TEntity : DBEntity_Base, IDBField_ID<TKey>, new()
    {
        private TDBContext _dbContext;
        public new TDBContext DBContext { get { return _dbContext; } protected set { _dbContext = value; base.DBContext = value; } }

        public DBRepository() { DBContext = (TDBContext)GetDBContext(); }

        protected override IDBContext GetDBContext() { return ServiceContext.Resolve<TDBContext>(); }
    }
}
