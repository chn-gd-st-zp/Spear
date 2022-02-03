using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using MessagePack;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.DTO
{
    /// <summary>
    /// 排序方式
    /// 0、2: AES、正序
    /// 1、3: DESC、倒序
    /// </summary>
    public enum Enum_SortDirection
    {
        [Remark("默认、无")]
        None = -1,

        [Remark("正序")]
        ASC = 0,

        [Remark("倒序")]
        DESC = 1,

        [Remark("正序")]
        Ascending = 2,

        [Remark("倒序")]
        Descending = 3,
    }

    /// <summary>
    /// 排序子对象
    /// </summary>
    [MessagePackObject(true)]
    public class IDTO_Sort
    {
        /// <summary>
        /// 字段名
        /// </summary>
        [Remark("字段名")]
        public string FieldName { get; set; }

        /// <summary>
        /// 排序方式
        /// 0、2: AES、正序
        /// 1、3: DESC、倒序
        /// </summary>
        [Remark("排序方式")]
        public Enum_SortDirection EDirection { get; set; }
    }

    /// <summary>
    /// 拓展方法
    /// </summary>
    public static class IDTO_Sort_Ext
    {
        public static string GenericOrderBySql<T>(this IDTO_Search param) where T : DBEntity_Base
        {
            string result = string.Empty;

            Type type = typeof(T);
            PropertyInfo[] propertyInfos = type.GetProperties();

            List<IDTO_Sort> idtoSorts = param == null || param.Sort == null ? new List<IDTO_Sort>() : param.Sort;

            if (idtoSorts == null || idtoSorts.Count() == 0)
            {
                var attr = type.GetDefaultSortField<T>();
                if (attr != null)
                {
                    result += result.IsEmptyString() ? string.Empty : ",";
                    result += attr.RealName + " ";

                    switch (attr.EDirection)
                    {
                        case Enum_SortDirection.ASC:
                        case Enum_SortDirection.Ascending:
                            result += "asc";
                            break;
                        case Enum_SortDirection.DESC:
                        case Enum_SortDirection.Descending:
                            result += "desc";
                            break;
                    }
                }
            }
            else
            {
                foreach (var idtoSort in idtoSorts)
                {
                    if (idtoSort.FieldName.IsEmptyString())
                        continue;

                    var sortFieldInfo = propertyInfos.GetSortField<T>(idtoSort);
                    if (sortFieldInfo == null)
                        continue;

                    result += result.IsEmptyString() ? string.Empty : ",";
                    result += sortFieldInfo.Item2 + " ";

                    switch (idtoSort.EDirection)
                    {
                        case Enum_SortDirection.ASC:
                        case Enum_SortDirection.Ascending:
                            result += "asc";
                            break;
                        case Enum_SortDirection.DESC:
                        case Enum_SortDirection.Descending:
                            result += "desc";
                            break;
                    }
                }
            }

            return result;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, IDTO_Search param) where T : DBEntity_Base
        {
            Type sourceType = typeof(T);
            PropertyInfo[] sourcePropertyInfos = sourceType.GetProperties();

            List<IDTO_Sort> idtoSorts = param == null || param.Sort == null ? new List<IDTO_Sort>() : param.Sort;

            if (idtoSorts.Count == 0)
            {
                var attr = sourceType.GetDefaultSortField<T>();
                if (attr != null)
                    idtoSorts.Add(new IDTO_Sort() { FieldName = attr.RealName, EDirection = attr.EDirection });
            }

            Type classType = typeof(IDTO_Sort_Ext);
            MethodInfo mi = classType.GetMethod("AppendExpression");

            bool firstAdd = true;
            foreach (var sortItem in idtoSorts)
            {
                if (sortItem.FieldName.IsEmptyString())
                    continue;

                var sortFieldInfo = sourcePropertyInfos.GetSortField<T>(sortItem);
                if (sortFieldInfo == null)
                    continue;

                query = mi.MakeGenericMethod(new[] { sourceType, sortFieldInfo.Item1.PropertyType })
                    .Invoke(null, new object[] { query, sortItem.EDirection, firstAdd, sourceType, sortFieldInfo.Item2 }) as IQueryable<T>;

                firstAdd = false;
            }

            return query;
        }

        public static IQueryable<T> AppendExpression<T, TKey>(this IQueryable<T> query, Enum_SortDirection eSortDirection, bool firstAdd, Type type, string fieldName)
        {
            ParameterExpression parameter = Expression.Parameter(type, "p");
            MemberExpression body = Expression.PropertyOrField(parameter, fieldName);
            Expression<Func<T, TKey>> expression = Expression.Lambda<Func<T, TKey>>(body, parameter);

            if (firstAdd)
            {
                if (eSortDirection == Enum_SortDirection.ASC || eSortDirection == Enum_SortDirection.Ascending)
                {
                    query = query.OrderBy(expression);
                }
                else
                {
                    query = query.OrderByDescending(expression);
                }
            }
            else
            {
                if (eSortDirection == Enum_SortDirection.ASC || eSortDirection == Enum_SortDirection.Ascending)
                {
                    query = ((IOrderedQueryable<T>)query).ThenBy(expression);
                }
                else
                {
                    query = ((IOrderedQueryable<T>)query).ThenByDescending(expression);
                }
            }

            return query;
        }
    }
}
