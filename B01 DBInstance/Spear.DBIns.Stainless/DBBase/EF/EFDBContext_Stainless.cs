using Microsoft.EntityFrameworkCore;

using Spear.DBIns.Stainless.Entity;
using Spear.Inf.EF;

namespace Spear.DBIns.Stainless
{
    public class EFDBContext_Stainless : EFDBContext
    {
        public EFDBContext_Stainless(EFDBContextOptionsBuilder<EFDBContext_Stainless> builder) : base(builder) { }

        public DbSet<TE_AgentOrder> TE_AgentOrder { get; set; }
        public DbSet<TE_CustomerOrder> TE_CustomerOrder { get; set; }

        protected override void InitDBSets()
        {
            AddDBSet(TE_AgentOrder);
            AddDBSet(TE_CustomerOrder);
        }

        protected override void BindMaps(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TE_AgentOrder_Map());
            modelBuilder.ApplyConfiguration(new TE_CustomerOrder_Map());
        }
    }
}
