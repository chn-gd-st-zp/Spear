using System;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.DTO;
using Spear.Inf.EF;

namespace Spear.Demo.DBIns.Stainless.Entity
{
    [Table("AgentOrder")]
    [DefaultSortField("AOID", Enum_SortDirection.ASC)]
    public class TB_AgentOrder : CommonData, IDBField_PrimeryKey<long>
    {
        [Column("AOID")]
        [JsonProperty("AOID")]
        public long PrimeryKey { get; set; }

        [Column("CreateTime")]
        [JsonProperty("CreateTime")]
        public override DateTime CreateTime { get; set; }
    }

    public class TB_AgentOrder_Map : EFDBEntityMapping<TB_AgentOrder>
    {
        public override void Configure(EntityTypeBuilder<TB_AgentOrder> builder)
        {
            builder.HasKey(att => att.PrimeryKey);
        }
    }
}
