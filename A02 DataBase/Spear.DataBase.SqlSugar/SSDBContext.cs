using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using SqlSugar;

using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.SqlSugar
{
    public delegate ConnectionConfig OptionBulidAction(ConnectionConfig connectionConfig);

    public delegate List<ConnectionConfig> OptionsBulidAction(List<ConnectionConfig> connectionConfigs);

    public class SSDBContextOptionBuilder : ConnectionConfig { public OptionBulidAction BulidAction { get; set; } }

    public class SSDBContextOptionsBuilder : List<ConnectionConfig> { public OptionsBulidAction BulidAction { get; set; } }

    public class SSDBConnectionConfig : ConnectionConfig { };

    public abstract class SSDBContext : SqlSugarClient, IDBContext
    {
        private string _id = Unique.GetGUID();
        public string ID { get { return _id; } }

        public SSDBContext(SSDBContextOptionBuilder optionsBuilder) : base(optionsBuilder.BulidAction(optionsBuilder)) { }

        public SSDBContext(SSDBContextOptionsBuilder optionsBuilder) : base(optionsBuilder.BulidAction(optionsBuilder)) { }

        public int SaveChanges() { return SaveQueues(); }

        public string GetTBName<TEntity>() where TEntity : class, IDBEntity, new()
        {
            return GetTBName(typeof(TEntity));
        }

        public string GetTBName(Type dbType)
        {
            var attr = dbType.GetCustomAttribute<SugarTable>();
            if (attr == default)
                return dbType.Name;

            return attr.TableName;
        }

        public string GetPKName<TEntity>() where TEntity : class, IDBEntity, new()
        {
            return GetPKName(typeof(TEntity));
        }

        public string GetPKName(Type dbType)
        {
            if (dbType == null)
                return string.Empty;

            if (!dbType.IsImplementedOf(typeof(IDBField_PrimeryKey<>)))
                return string.Empty;

            var obj = InstanceCreator.Create(dbType) as IDBField_PrimeryKey;
            if (obj == null)
                return string.Empty;

            var property = obj.GetPKPropertyInfo();
            if (property == null)
                return string.Empty;

            var attr = property.GetCustomAttribute<SugarColumn>();
            if (attr == null)
                return property.Name;

            return attr.ColumnName;
        }

        public object GetQueryable<TEntity>() where TEntity : class, IDBEntity, new()
        {
            return GetSimpleClient<TEntity>().AsQueryable();
        }

        #region 增

        public bool Create<TEntity>(TEntity obj, bool save = true) where TEntity : class, IDBEntity, new()
        {
            return GetSimpleClient<TEntity>().Insert(obj);
        }

        public bool Create<TEntity>(IEnumerable<TEntity> objs, bool save = true) where TEntity : class, IDBEntity, new()
        {
            return GetSimpleClient<TEntity>().InsertRange(objs.ToList());
        }

        #endregion

        #region 删

        public bool Delete<TEntity>(TEntity obj, bool save = true) where TEntity : class, IDBEntity, new()
        {
            return GetSimpleClient<TEntity>().Delete(obj);
        }

        public bool Delete<TEntity>(IEnumerable<TEntity> objs, bool save = true) where TEntity : class, IDBEntity, new()
        {
            return Deleteable(objs.ToList()).ExecuteCommand() > 0;
        }

        public bool Delete<TEntity, TKey>(TKey key) where TEntity : class, IDBEntity, IDBField_PrimeryKey<TKey>, new()
        {
            return GetSimpleClient<TEntity>().DeleteById(key);
        }

        public bool Delete<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, IDBEntity, new()
        {
            return GetSimpleClient<TEntity>().Delete(expression);
        }

        #endregion

        #region 改

        public bool Update<TEntity>(TEntity obj, bool save = true) where TEntity : class, IDBEntity, new()
        {
            return GetSimpleClient<TEntity>().Update(obj);
        }

        public bool Update<TEntity>(IEnumerable<TEntity> objs, bool save = true) where TEntity : class, IDBEntity, new()
        {
            return GetSimpleClient<TEntity>().UpdateRange(objs.ToList());
        }

        #endregion

        #region 查 - 单个

        public TEntity Single<TEntity>(object key) where TEntity : class, IDBEntity, new()
        {
            return GetSimpleClient<TEntity>().GetById(key);
        }

        public TEntity Single<TEntity, TKey>(TKey key) where TEntity : class, IDBEntity, IDBField_PrimeryKey<TKey>, new()
        {
            return GetSimpleClient<TEntity>().GetById(key);
        }

        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, IDBEntity, new()
        {
            return GetSimpleClient<TEntity>().GetSingle(expression);
        }

        #endregion

        #region 查 - 列表

        public List<TEntity> List<TEntity, TKey>(params TKey[] keys) where TEntity : class, IDBEntity, IDBField_PrimeryKey<TKey>, new()
        {
            var query = GetSimpleClient<TEntity>().AsQueryable();

            if (keys != null && keys.Length != 0)
                query = query.Where(o => keys.Contains(o.PrimeryKey));

            return query.ToList();
        }

        public List<TEntity> List<TEntity>(Expression<Func<TEntity, bool>> expression = null, IDTO_List param = null) where TEntity : class, IDBEntity, new()
        {
            var query = GetSimpleClient<TEntity>().AsQueryable();

            if (expression != null)
                query = query.Where(expression);

            if (param != null)
                query = query.OrderBy(param.GenericOrderBySql<TEntity>());

            return query.ToList();
        }

        public List<TEntity> ListByQueryable<TEntity>(object queryObj, IDTO_List param = null) where TEntity : class, IDBEntity, new()
        {
            string orderBy = param.GenericOrderBySql<TEntity>();

            return ((ISugarQueryable<TEntity>)queryObj).OrderBy(orderBy).ToList();
        }

        #endregion

        #region 查 - 分页

        public Tuple<List<TEntity>, int> Page<TEntity>(Expression<Func<TEntity, bool>> expression, IDTO_Page param = null) where TEntity : class, IDBEntity, new()
        {
            var query = GetSimpleClient<TEntity>().AsQueryable();
            if (expression != null)
                query = query.Where(expression);

            return PageByQueryable<TEntity>(query, param);
        }

        public Tuple<List<TEntity>, int> PageByQueryable<TEntity>(object queryObj, IDTO_Page param = null) where TEntity : class, IDBEntity, new()
        {
            string orderBy = param.GenericOrderBySql<TEntity>();

            var query = queryObj == null ? GetSimpleClient<TEntity>().AsQueryable() : (ISugarQueryable<TEntity>)queryObj;

            query = query.OrderBy(orderBy);

            int rowQty = query.Count();

            param.PageSize = param.PageSize == 0 ? 10 : param.PageSize;
            param.PageIndex = param.PageIndex == 0 ? 1 : param.PageIndex;

            var dataList = query.ToPageList(param.PageIndex, param.PageSize, ref rowQty);

            return new Tuple<List<TEntity>, int>(dataList, rowQty);
        }

        #endregion

        #region 查 - 序号

        public string GetNextSequence<TEntity>() where TEntity : class, IDBEntity, IDBField_Sequence, new()
        {
            var query = GetSimpleClient<TEntity>().AsQueryable();

            var obj = query.OrderBy(o => o.CurSequence, OrderByType.Desc).Single();
            if (obj == null)
                obj = new TEntity();

            return obj.GetSequence(1);
        }

        #endregion

        #region 执行 SQL

        public int ExecuteSql(string sql, params DBParameter[] paramArray)
        {
            return Ado.ExecuteCommand(sql, paramArray.Parse());
        }

        public List<IDBEntity> SelectFromSql(Type dbType, string sql, params DBParameter[] paramArray)
        {
            return Ado.SqlQuery<dynamic>(sql, paramArray.Parse())
                .Select(o => o as IDBEntity)
                .ToList();
        }

        public List<TEntity> SelectFromSql<TEntity>(string sql, params DBParameter[] paramArray) where TEntity : class, IDBEntity, new()
        {
            return Ado.SqlQuery<TEntity>(sql, paramArray.Parse());
        }

        public int ExecuteStoredProcedure(string sql, params DBParameter[] paramArray)
        {
            return Ado.UseStoredProcedure().ExecuteCommand(sql, paramArray.Parse());
        }

        public List<TEntity> SelectFromStoredProcedure<TEntity>(string sql, params DBParameter[] paramArray) where TEntity : class, IDBEntity, new()
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
