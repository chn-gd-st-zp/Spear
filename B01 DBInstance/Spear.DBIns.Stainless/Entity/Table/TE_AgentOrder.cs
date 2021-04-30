﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.DTO;
using Spear.Inf.EF;

namespace Spear.DBIns.Stainless.Entity
{
    [Table("AgentOrder")]
    public class TE_AgentOrder : CommonData
    {
        [DefaultSortField("AOID", Enum_SortDirection.ASC)]
        [Column("AOID")]
        [JsonProperty("AOID")]
        public long ID { get; set; }

        [Column("CreateTime")]
        [JsonProperty("CreateTime")]
        public override DateTime CreateTime { get; set; }
    }

    public class TE_AgentOrder_Map : EFDBEntityMapping<TE_AgentOrder>
    {
        public override void Configure(EntityTypeBuilder<TE_AgentOrder> builder)
        {
            builder.HasKey(att => att.ID);
        }
    }
}
