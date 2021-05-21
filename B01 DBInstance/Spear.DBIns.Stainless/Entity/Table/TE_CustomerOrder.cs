using System;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.DTO;
using Spear.Inf.EF;

namespace Spear.DBIns.Stainless.Entity
{
    [Table("CustomerOrder")]
    public class TE_CustomerOrder : CommonData, IDBField_ID<string>
    {
        [DefaultSortField("COID", Enum_SortDirection.ASC)]
        [Column("COID")]
        [JsonProperty("COID")]
        public string ID { get; set; }

        [Column("CreateTime")]
        [JsonProperty("CreateTime")]
        public override DateTime CreateTime { get; set; }
    }

    public class TE_CustomerOrder_Map : EFDBEntityMapping<TE_CustomerOrder>
    {
        public override void Configure(EntityTypeBuilder<TE_CustomerOrder> builder)
        {
            builder.HasKey(att => att.ID);
        }
    }
}
