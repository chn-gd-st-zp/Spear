using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.DTO;

namespace Spear.Inf.Core.Interface
{
    public interface IDBFunc4DBContext
    {
        #region 增

        bool Create<TEntity>(TEntity obj, bool save = true) where TEntity : DBEntity_Basic, new();

        bool Create<TEntity>(IEnumerable<TEntity> objs, bool save = true) where TEntity : DBEntity_Basic, new();

        #endregion

        #region 删

        bool Delete<TEntity, TKey>(TKey key) where TEntity : DBEntity_Basic, new();

        bool Delete<TEntity>(TEntity obj, bool save = true) where TEntity : DBEntity_Basic, new();

        bool Delete<TEntity>(IEnumerable<TEntity> objs, bool save = true) where TEntity : DBEntity_Basic, new();

        bool Delete<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : DBEntity_Basic, new();

        #endregion

        #region 改

        bool Update<TEntity>(TEntity obj, bool save = true) where TEntity : DBEntity_Basic, new();

        bool Update<TEntity>(IEnumerable<TEntity> objs, bool save = true) where TEntity : DBEntity_Basic, new();

        #endregion

        #region 查 - 单个

        TEntity Single<TEntity, TKey>(TKey key) where TEntity : DBEntity_Basic, new();

        TEntity Single<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : DBEntity_Basic, new();

        #endregion

        #region 查 - 列表

        List<TEntity> List<TEntity>(Expression<Func<TEntity, bool>> expression = null, IDTO_List param = null) where TEntity : DBEntity_Basic, new();

        List<TEntity> ListByQueryable<TEntity>(object queryObj, IDTO_List param = null) where TEntity : DBEntity_Basic, new();

        #endregion

        #region 查 - 分页

        Tuple<List<TEntity>, int> Page<TEntity>(Expression<Func<TEntity, bool>> expression, IDTO_Page param = null) where TEntity : DBEntity_Basic, new();

        Tuple<List<TEntity>, int> PageByQueryable<TEntity>(object queryObj, IDTO_Page param = null) where TEntity : DBEntity_Basic, new();

        #endregion

        #region 执行 SQL

        int ExecuteSql(string sql, params DBParameter[] paramArray);

        List<TEntity> SelectFromSql<TEntity>(string sql, params DBParameter[] paramArray) where TEntity : DBEntity_Basic, new();

        int ExecuteStoredProcedure(string sql, params DBParameter[] paramArray);

        List<TEntity> SelectFromStoredProcedure<TEntity>(string sql, params DBParameter[] paramArray) where TEntity : DBEntity_Basic, new();

        #endregion
    }

    public interface IDBFunc4Repository
    {
        #region 查 - 列表

        List<T> List<T>(object queryObj, IDTO_List param = null) where T : DBEntity_Basic, new();

        #endregion

        #region 查 - 分页

        Tuple<List<T>, int> PageByQueryable<T>(object queryObj, IDTO_Page param = null) where T : DBEntity_Basic, new();

        #endregion

        #region 执行 SQL

        int ExecuteSql(string sql, params DBParameter[] paramArray);

        List<T> SelectFromSql<T>(string sql, params DBParameter[] paramArray) where T : DBEntity_Basic, new();

        int ExecuteStoredProcedure(string sql, params DBParameter[] paramArray);

        List<T> SelectFromStoredProcedure<T>(string sql, params DBParameter[] paramArray) where T : DBEntity_Basic, new();

        #endregion
    }

    public interface IDBFunc4Repository<TEntity> : IDBFunc4Repository where TEntity : DBEntity_Basic, new()
    {
        #region 增

        bool Create(TEntity obj);

        bool Create(IEnumerable<TEntity> objs);

        #endregion

        #region 删

        bool Delete(TEntity obj);

        bool Delete(IEnumerable<TEntity> objs);

        bool Delete(Expression<Func<TEntity, bool>> expression);

        #endregion

        #region 改

        bool Update(TEntity obj);

        bool Update(IEnumerable<TEntity> objs);

        #endregion

        #region 查 - 单个

        TEntity Single(Expression<Func<TEntity, bool>> expression);

        #endregion

        #region 查 - 列表

        List<TEntity> List(Expression<Func<TEntity, bool>> expression, IDTO_List param = null);

        #endregion

        #region 查 - 分页

        Tuple<List<TEntity>, int> Page(Expression<Func<TEntity, bool>> expression, IDTO_Page param = null);

        #endregion
    }

    public interface IDBFunc4Repository<TEntity, TKey> : IDBFunc4Repository<TEntity> where TEntity : DBEntity_Basic, new()
    {
        #region 删

        bool Delete(TKey key);

        #endregion

        #region 查 - 单个

        TEntity Single(TKey key);

        #endregion
    }
}
