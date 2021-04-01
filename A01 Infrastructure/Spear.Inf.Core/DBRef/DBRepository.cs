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

        public DBRepository()
        {
            DBContext = GetDBContext();
        }

        protected virtual IDBContext GetDBContext()
        {
            return ServiceContext.ResolveServ<IDBContext>();
        }

        #region 查 - 列表

        public List<T> List<T>(object queryObj, IDTO_List param = null) where T : DBEntity_Basic, new()
        {
            return DBContext.ListByQueryable<T>(queryObj, param);
        }

        #endregion

        #region 查 - 分页

        public Tuple<List<T>, int> PageByQueryable<T>(object queryObj, IDTO_Page param = null) where T : DBEntity_Basic, new()
        {
            return DBContext.PageByQueryable<T>(queryObj, param);
        }

        #endregion

        #region 执行 SQL

        public int ExecuteSql(string sql, params DBParameter[] paramArray)
        {
            return DBContext.ExecuteSql(sql, paramArray);
        }

        public List<T> SelectFromSql<T>(string sql, params DBParameter[] paramArray) where T : DBEntity_Basic, new()
        {
            return DBContext.SelectFromSql<T>(sql, paramArray);
        }

        public int ExecuteStoredProcedure(string sql, params DBParameter[] paramArray)
        {
            return DBContext.ExecuteStoredProcedure(sql, paramArray);
        }

        public List<T> SelectFromStoredProcedure<T>(string sql, params DBParameter[] paramArray) where T : DBEntity_Basic, new()
        {
            return DBContext.SelectFromStoredProcedure<T>(sql, paramArray);
        }

        #endregion
    }

    public class DBRepository<TEntity> : DBRepository, IDBRepository<TEntity>
        where TEntity : DBEntity_Basic, new()
    {
        #region 增

        public bool Create(TEntity obj)
        {
            return DBContext.Create(obj);
        }

        public bool Create(IEnumerable<TEntity> objs)
        {
            return DBContext.Create(objs);
        }

        #endregion

        #region 删

        public bool Delete(TEntity obj)
        {
            return DBContext.Delete(obj);
        }

        public bool Delete(IEnumerable<TEntity> objs)
        {
            return DBContext.Delete(objs);
        }

        public bool Delete(Expression<Func<TEntity, bool>> expression)
        {
            return DBContext.Delete(expression);
        }

        #endregion

        #region 改

        public bool Update(TEntity obj)
        {
            return DBContext.Update(obj);
        }

        public bool Update(IEnumerable<TEntity> objs)
        {
            return DBContext.Update(objs);
        }

        #endregion

        #region 查 - 单个

        public TEntity Single(Expression<Func<TEntity, bool>> expression)
        {
            return DBContext.Single(expression);
        }

        #endregion

        #region 查 - 列表

        public List<TEntity> List(Expression<Func<TEntity, bool>> expression, IDTO_List param = null)
        {
            return DBContext.List(expression, param);
        }

        #endregion

        #region 查 - 分页

        public Tuple<List<TEntity>, int> Page(Expression<Func<TEntity, bool>> expression, IDTO_Page param = null)
        {
            return DBContext.Page(expression, param);
        }

        #endregion
    }

    public class DBRepository<TEntity, TKey> : DBRepository<TEntity>, IDBRepository<TEntity, TKey>
        where TEntity : DBEntity_Basic, new()
    {
        #region 删

        public bool Delete(TKey key)
        {
            return DBContext.Delete<TEntity, TKey>(key);
        }

        #endregion

        #region 查 - 单个

        public TEntity Single(TKey key)
        {
            return DBContext.Single<TEntity, TKey>(key);
        }

        #endregion
    }

    public class DBRepository<TDBContext, TEntity, TKey> : DBRepository<TEntity, TKey>, IDBRepository<TDBContext, TEntity, TKey>
        where TDBContext : IDBContext
        where TEntity : DBEntity_Basic, new()
    {
        private TDBContext _dbContext;
        public new TDBContext DBContext { get { return _dbContext; } set { _dbContext = value; base.DBContext = value; } }

        public DBRepository()
        {
            DBContext = (TDBContext)GetDBContext();
        }

        protected override IDBContext GetDBContext()
        {
            return ServiceContext.ResolveServ<TDBContext>();
        }
    }
}
