using Microsoft.EntityFrameworkCore;

using Spear.Inf.EF;

using Spear.Demo.DBIns.Stainless.Entity;

namespace Spear.Demo.DBIns.Stainless
{
    public class EFDBContext_Stainless : EFDBContext
    {
        public EFDBContext_Stainless(EFDBContextOptionsBuilder<EFDBContext_Stainless> builder) : base(builder) { }

        public DbSet<TB_AgentOrder> TB_AgentOrder { get; set; }
        public DbSet<TB_CustomerOrder> TB_CustomerOrder { get; set; }

        protected override void InitDBSets()
        {
            AddDBSet(TB_AgentOrder);
            AddDBSet(TB_CustomerOrder);
        }

        protected override void BindMaps(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TB_AgentOrder_Map());
            modelBuilder.ApplyConfiguration(new TB_CustomerOrder_Map());
        }
    }
}
