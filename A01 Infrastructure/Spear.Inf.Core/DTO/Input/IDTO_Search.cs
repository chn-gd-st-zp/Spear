using System.Collections.Generic;

using MessagePack;

using Spear.Inf.Core.Attr;

namespace Spear.Inf.Core.DTO
{
    /// <summary>
    /// 条件查询
    /// </summary>
    [MessagePackObject(true)]
    public class IDTO_Search : IDTO_Input
    {
        /// <summary>
        /// 排序
        /// </summary>
        [Remark("排序")]
        public List<IDTO_Sort> Sort { get; set; }
    }

    /// <summary>
    /// 条件查询 - 列表专用
    /// </summary>
    [MessagePackObject(true)]
    public class IDTO_List : IDTO_Search
    {
        //
    }

    /// <summary>
    /// 条件查询 - 分页专用
    /// </summary>
    [MessagePackObject(true)]
    public class IDTO_Page : IDTO_Search
    {
        /// <summary>
        /// 当前页
        /// </summary>
        [Remark("当前页")]
        public int PageIndex { get; set; }

        /// <summary>
        /// 页条数
        /// </summary>
        [Remark("页条数")]
        public int PageSize { get; set; }
    }
}