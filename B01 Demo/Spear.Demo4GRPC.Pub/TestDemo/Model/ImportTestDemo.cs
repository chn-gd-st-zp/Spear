using System;

using Spear.Inf.DataFile.Excel;

namespace Spear.Demo4GRPC.Pub.TestDemo
{
    [ImportSheet("demo")]
    public class ImportTestDemo
    {
        /// <summary>
        /// ID
        /// </summary>
        [ImportCol(typeof(string), "ID")]
        public string ID { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        [ImportCol(typeof(string), "节点名称")]
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [ImportCol(typeof(DateTime), "创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 当前节点代码
        /// </summary>
        [ImportCol(typeof(string), "当前节点代码")]
        public string CurNode { get; set; }

        /// <summary>
        /// 父级节点代码
        /// </summary>
        [ImportCol(typeof(string), "父级节点代码")]
        public string ParentNode { get; set; }

        /// <summary>
        /// 完整节点代码
        /// </summary>
        [ImportCol(typeof(string), "完整节点代码")]
        public string FullNode { get; set; }

        /// <summary>
        /// 当前节点排序
        /// </summary>
        [ImportCol(typeof(string), "当前节点排序")]
        public string CurSequence { get; set; }

        /// <summary>
        /// 完整节点排序
        /// </summary>
        [ImportCol(typeof(string), "完整节点排序")]
        public string FullSequence { get; set; }

        /// <summary>
        /// 节点状态
        /// </summary>
        [ImportCol(typeof(string), "节点状态")]
        public string Status { get; set; }
    }
}
