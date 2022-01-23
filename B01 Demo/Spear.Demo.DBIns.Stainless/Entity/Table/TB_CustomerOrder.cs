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
    [Table("CustomerOrder")]
    [DefaultSortField("COID", Enum_SortDirection.ASC)]
    public class TB_CustomerOrder : CommonData, IDBField_PrimeryKey<string>
    {
        [Column("COID")]
        [JsonProperty("COID")]
        public string PrimeryKey { get; set; }

        [Column("CreateTime")]
        [JsonProperty("CreateTime")]
        public override DateTime CreateTime { get; set; }
    }

    public class TB_CustomerOrder_Map : EFDBEntityMapping<TB_CustomerOrder>
    {
        public override void Configure(EntityTypeBuilder<TB_CustomerOrder> builder)
        {
            builder.HasKey(att => att.PrimeryKey);
        }
    }
}
