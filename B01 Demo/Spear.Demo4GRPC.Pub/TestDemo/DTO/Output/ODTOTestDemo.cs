using System;

using MessagePack;
using Newtonsoft.Json;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.Tool;

namespace Spear.Demo4GRPC.Pub.TestDemo
{
    [MessagePackObject(true)]
    public class ODTOTestDemo : IDBField_TreeNSequence<string>
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 当前节点代码
        /// </summary>
        public string CurNode { get; set; }

        /// <summary>
        /// 父级节点代码
        /// </summary>
        public string ParentNode { get; set; }

        /// <summary>
        /// 完整节点代码
        /// </summary>
        public string FullNode { get; set; }

        /// <summary>
        /// 当前节点排序
        /// </summary>
        public string CurSequence { get; set; }

        /// <summary>
        /// 完整节点排序
        /// </summary>
        public string FullSequence { get; set; }

        /// <summary>
        /// 节点状态
        /// </summary>
        [JsonIgnore]
        public string Status { get; set; }

        /// <summary>
        /// 节点状态
        /// </summary>
        public Enum_Status EStatus { get { return Status.Convert2Enum<Enum_Status>(); } }
    }
}
