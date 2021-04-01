using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;
using MessagePack;

using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core.DTO
{
    [MessagePackObject(true)]
    public class ODTO_Page<T>
    {
        public ODTO_Page() { }

        /// <summary>
        /// 每页几条
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 第几页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 共几页
        /// </summary>
        public int TotalQty_Page { get; set; }

        /// <summary>
        /// 共几条
        /// </summary>
        public int TotalQty_Row { get; set; }

        /// <summary>
        /// 数据集合
        /// </summary>
        public List<T> Data { get; set; }
    }

    public static class ODTO_Page_Ext
    {
        public static ODTO_Page<T> ToODTOPage<T>(this List<T> dataList, int rowQty, IDTO_Page pageParam)
        {
            ODTO_Page<T> result = new ODTO_Page<T>();

            result.PageSize = pageParam.PageSize;
            result.PageIndex = pageParam.PageIndex;
            result.TotalQty_Row = rowQty;
            result.Data = dataList;

            result.TotalQty_Page = (result.TotalQty_Row / result.PageSize) + (result.TotalQty_Row % result.PageSize == 0 ? 0 : 1);

            return result;
        }

        public static ODTO_Page<T> ToODTOPage<T>(this Tuple<List<T>, int> pageData, IDTO_Page pageParam)
        {
            var result = pageData.Item1.ToODTOPage(pageData.Item2, pageParam);
            return result;
        }

        public static ODTO_Page<TTarget> ToODTOPage<TSource, TTarget>(this Tuple<List<TSource>, int> pageData, IDTO_Page pageParam)
        {
            var mapper = ServiceContext.ResolveServ<IMapper>();
            var dataList = pageData.Item1.Select(o => mapper.Map<TTarget>(o)).ToList();

            var result = dataList.ToODTOPage(pageData.Item2, pageParam);
            return result;
        }

        public static ODTO_Page<TTarget> ToODTOPage<TSource, TTarget>(this Tuple<List<TSource>, int> pageData, IDTO_Page pageParam, Func<TSource, TTarget> selector)
        {
            var dataList = pageData.Item1.Select(selector).ToList();

            var result = dataList.ToODTOPage(pageData.Item2, pageParam);
            return result;
        }
    }
}
