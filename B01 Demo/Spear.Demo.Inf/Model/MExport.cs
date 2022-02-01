using System;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Tool;
using Spear.Inf.DataFile.Excel;

namespace Spear.Demo.Inf.Model
{
    [ExportSheet("demo")]
    public class MExport
    {
        public decimal Num { get { return 1; } }

        [ExportContent(1, "ID", typeof(string))]
        public string ID { get; set; }

        [ExportAggregate(1, "ID", typeof(string), Enum_AggregateType.Normal)]
        public string ID2 { get { return string.Empty; } set { } }


        [ExportContent(2, "节点名称", typeof(string))]
        public string Name { get; set; }

        [ExportAggregate(2, "节点名称", typeof(string), Enum_AggregateType.Normal)]
        public string Name2 { get { return string.Empty; } set { } }


        [ExportContent(3, "创建时间", typeof(DateTime))]
        public DateTime CreateTime { get; set; }

        [ExportAggregate(3, "创建时间", typeof(string), Enum_AggregateType.Normal)]
        public string CreateTime2 { get { return string.Empty; } set { } }


        [ExportContent(4, "当前节点代码", typeof(string))]
        public string CurNode { get; set; }

        [ExportAggregate(4, "当前节点代码", typeof(string), Enum_AggregateType.Normal)]
        public string CurNode2 { get { return string.Empty; } set { } }


        [ExportContent(5, "父级节点代码", typeof(string))]
        public string ParentNode { get; set; }

        [ExportAggregate(5, "父级节点代码", typeof(string), Enum_AggregateType.Normal)]
        public string ParentNode2 { get { return string.Empty; } set { } }


        [ExportContent(6, "完整节点代码", typeof(string))]
        public string FullNode { get; set; }

        [ExportAggregate(6, "完整节点代码", typeof(string), Enum_AggregateType.Normal)]
        public string FullNode2 { get { return string.Empty; } set { } }


        [ExportContent(7, "当前节点排序", typeof(string))]
        public string CurSequence { get; set; }

        [ExportAggregate(7, "当前节点排序", typeof(string), Enum_AggregateType.Normal)]
        public string CurSequence2 { get { return string.Empty; } set { } }


        [ExportContent(8, "完整节点排序", typeof(string))]
        public string FullSequence { get; set; }

        [ExportAggregate(8, "完整节点排序", typeof(string), Enum_AggregateType.Normal)]
        public string FullSequence2 { get { return string.Empty; } set { } }


        [ExportContent(9, "节点状态", typeof(string))]
        public Enum_Status EStatus { get; set; }

        [ExportAggregate(9, "节点状态", typeof(string), Enum_AggregateType.Normal)]
        public string EStatus2 { get { return "总计"; } set { } }


        [ExportContent(10, "总额", typeof(decimal), "0.00")]
        [ExportAggregate(10, "总额", typeof(decimal), Enum_AggregateType.Sum)]
        public decimal Amt { get; set; }


        [ExportContent(11, "消耗", typeof(decimal), "0.00")]
        [ExportAggregate(11, "消耗", typeof(decimal), Enum_AggregateType.Sum)]
        public decimal Use { get; set; }


        [ExportContent(12, "消耗占比", typeof(decimal), "0.00", Suffix = "%", Enlargement = 100)]
        public decimal Rate { get; set; }

        [ExportAggregate(12, "消耗占比", typeof(decimal), Enum_AggregateType.Normal, "0.00", Suffix = "%", Enlargement = 100)]
        public decimal? Rate2 { get { return MathConverter.Divider(Use, Amt); } set { } }
    }
}
