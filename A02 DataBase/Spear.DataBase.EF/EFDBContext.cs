﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.EF
{
    public abstract class EFDBContext : DbContext, IDBContext
    {
        public string ID { get { return _id; } }
        private string _id = Unique.GetGUID();

        public EFDBContext(DbContextOptions options) : base(options)
        {
            DBSets = new Dictionary<Type, object>();

            InitDBSets();
        }

        #region 对象映射

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BindMaps(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        protected abstract void BindMaps(ModelBuilder modelBuilder);

        #endregion

        #region DBSet方法

        private readonly Dictionary<Type, object> DBSets;

        protected abstract void InitDBSets();

        protected void AddDBSet<TEntity>(DbSet<TEntity> obj) where TEntity : DBEntity_Base, new()
        {
            var key = typeof(TEntity);
            var value = obj;

            DBSets.Add(key, value);
        }

        public DbSet<TEntity> GetDBSet<TEntity>() where TEntity : DBEntity_Base, new()
        {
            return DBSets[typeof(TEntity)] as DbSet<TEntity>;
        }

        #endregion

        #region 增

        public bool Create<TEntity>(TEntity obj, bool save = true) where TEntity : DBEntity_Base, new()
        {
            if (obj == null)
                return false;

            GetDBSet<TEntity>().Add(obj);

            return save ? SaveChanges() == 1 : true;
        }

        public bool Create<TEntity>(IEnumerable<TEntity> objs, bool save = true) where TEntity : DBEntity_Base, new()
        {
            if (objs == null || objs.Count() == 0)
                return false;
            GetDBSet<TEntity>().AddRange(objs);

            return save ? SaveChanges() == objs.Count() : true;
        }

        #endregion

        #region 删

        public bool Delete<TEntity, TKey>(TKey key) where TEntity : DBEntity_Base, IDBField_ID<TKey>, new()
        {
            var obj = Single<TEntity, TKey>(key);
            return Delete(obj);
        }

        public bool Delete<TEntity>(TEntity obj, bool save = true) where TEntity : DBEntity_Base, new()
        {
            if (obj == null)
                return false;

            GetDBSet<TEntity>().Remove(obj);

            return save ? SaveChanges() == 1 : true;
        }

        public bool Delete<TEntity>(IEnumerable<TEntity> objs, bool save = true) where TEntity : DBEntity_Base, new()
        {
            if (objs == null || objs.Count() == 0)
                return false;

            GetDBSet<TEntity>().RemoveRange(objs);

            return save ? SaveChanges() == objs.Count() : true;
        }

        public bool Delete<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : DBEntity_Base, new()
        {
            var objs = List(expression);
            return Delete(objs);
        }

        #endregion

        #region 改

        public bool Update<TEntity>(TEntity obj, bool save = true) where TEntity : DBEntity_Base, new()
        {
            if (obj == null)
                return false;

            GetDBSet<TEntity>().Update(obj);

            return save ? SaveChanges() == 1 : true;
        }

        public bool Update<TEntity>(IEnumerable<TEntity> objs, bool save = true) where TEntity : DBEntity_Base, new()
        {
            if (objs == null || objs.Count() == 0)
                return false;

            GetDBSet<TEntity>().UpdateRange(objs);

            return save ? SaveChanges() == objs.Count() : true;
        }

        #endregion

        #region 查 - 单个

        public TEntity Single<TEntity, TKey>(TKey key) where TEntity : DBEntity_Base, IDBField_ID<TKey>, new()
        {
            return Find<TEntity>(key);
        }

        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : DBEntity_Base, new()
        {
            return GetDBSet<TEntity>().Where(expression).SingleOrDefault();
        }

        #endregion

        #region 查 - 列表

        public List<TEntity> List<TEntity, TKey>(params TKey[] keys) where TEntity : DBEntity_Base, IDBField_ID<TKey>, new()
        {
            var query = GetDBSet<TEntity>().AsQueryable();

            if (keys != null && keys.Length != 0)
                query = query.Where(o => keys.Contains(o.ID));

            return query.ToList();
        }

        public List<TEntity> List<TEntity>(Expression<Func<TEntity, bool>> expression = null, IDTO_List param = null) where TEntity : DBEntity_Base, new()
        {
            var query = GetDBSet<TEntity>().AsQueryable();

            if (expression != null)
                query = query.Where(expression);

            if (param != null)
                query = query.OrderBy(param);

            return query.ToList();
        }

        public List<TEntity> ListByQueryable<TEntity>(object queryObj, IDTO_List param = null) where TEntity : DBEntity_Base, new()
        {
            return ((IQueryable<TEntity>)queryObj).OrderBy(param).ToList();
        }

        #endregion

        #region 查 - 分页

        public Tuple<List<TEntity>, int> Page<TEntity>(Expression<Func<TEntity, bool>> expression, IDTO_Page param = null) where TEntity : DBEntity_Base, new()
        {
            var query = GetDBSet<TEntity>().AsQueryable();
            if (expression != null)
                query = query.Where(expression);

            return PageByQueryable<TEntity>(query, param);
        }

        public Tuple<List<TEntity>, int> PageByQueryable<TEntity>(object queryObj, IDTO_Page param = null) where TEntity : DBEntity_Base, new()
        {
            var query = queryObj == null ? GetDBSet<TEntity>().AsQueryable() : (IQueryable<TEntity>)queryObj;

            query = query.OrderBy(param);

            int rowQty = query.Count();

            param.PageSize = param.PageSize == 0 ? 10 : param.PageSize;
            param.PageIndex = param.PageIndex == 0 ? 1 : param.PageIndex;

            var dataList = query.Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).ToList();

            return new Tuple<List<TEntity>, int>(dataList, rowQty);
        }

        #endregion

        #region 查 - 序号

        public string GetNextSequence<TEntity>() where TEntity : DBEntity_Base, IDBField_Sequence, new()
        {
            var query = GetDBSet<TEntity>();

            var obj = query.OrderByDescending(o => o.CurSequence).FirstOrDefault();
            if (obj == null)
                obj = new TEntity();

            return obj.GetSort(1);
        }

        #endregion

        #region 执行 SQL

        public int ExecuteSql(string sql, params DBParameter[] paramArray)
        {
            return Database.ExecuteSqlRaw(sql, paramArray.Parse());
        }

        public List<TEntity> SelectFromSql<TEntity>(string sql, params DBParameter[] paramArray) where TEntity : DBEntity_Base, new()
        {
            return this.Query<TEntity>(CommandType.Text, sql, paramArray.Parse());
        }

        public int ExecuteStoredProcedure(string sql, params DBParameter[] paramArray)
        {
            return Database.ExecuteSqlRaw(sql, paramArray.Parse());
        }

        public List<TEntity> SelectFromStoredProcedure<TEntity>(string sql, params DBParameter[] paramArray) where TEntity : DBEntity_Base, new()
        {
            return this.Query<TEntity>(CommandType.StoredProcedure, sql, paramArray.Parse());
        }

        #endregion
    }

    public abstract class EFDBEntityMapping<TDBEntity> : IEntityTypeConfiguration<TDBEntity> where TDBEntity : DBEntity_Base
    {
        public abstract void Configure(EntityTypeBuilder<TDBEntity> builder);
    }

    public static class DBContextExtend
    {
        public static List<SqlParameter> Parse(this DBParameter[] paramArray)
        {
            if (paramArray == null)
                return new List<SqlParameter>();

            return paramArray
                .Select(o => new SqlParameter
                {
                    ParameterName = o.Name,
                    Value = o.Value,
                    IsNullable = o.IsNullable,
                    Direction = o.Direction,
                    DbType = o.DBType,
                    Size = o.Size,
                })
                .ToList();
        }

        public static bool IsColumnExist(this DbDataReader dr, string columnName)
        {
            dr.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" + columnName + "'";
            return (dr.GetSchemaTable().DefaultView.Count > 0);
        }

        public static IQueryable<T> Query<T>(this EFDBContext dbContext) where T : DBEntity_Base, new()
        {
            return dbContext.GetDBSet<T>().AsQueryable<T>();
        }

        public static List<T> Query<T>(this EFDBContext dbContext, CommandType eCommandType, string sql, List<SqlParameter> paramList) where T : DBEntity_Base, new()
        {
            var result = new List<T>();

            var type = typeof(T);

            using (var connection = dbContext.Database.GetDbConnection())
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandType = eCommandType;
                cmd.CommandText = sql;

                if (paramList != null && paramList.Count() != 0)
                {
                    paramList.ForEach(o =>
                    {
                        if (o.IsNullable && o.Value == null)
                            o.Value = DBNull.Value;
                    });

                    cmd.Parameters.AddRange(paramList.ToArray());
                }

                connection.Open();

                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var columnSchema = reader.GetColumnSchema();

                        while (reader.Read())
                        {
                            T item = new T();

                            foreach (var pi in type.GetProperties())
                            {
                                object value = null;

                                if (reader.IsColumnExist(pi.Name))
                                {
                                    object obj_regular = reader[pi.Name];
                                    object obj_finnal = obj_regular == DBNull.Value ? null : obj_regular.TypeTo(pi.PropertyType);
                                    value = obj_finnal;
                                }

                                pi.SetValue(item, value);
                            }

                            result.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }
    }
}
