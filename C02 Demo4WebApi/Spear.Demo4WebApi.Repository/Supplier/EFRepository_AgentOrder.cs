using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Demo4WebApi.Base;

using Ent = Spear.DBIns.Stainless.Entity;

namespace Spear.Demo4WebApi.Repository
{
    [DIModeForService(Enum_DIType.SpecificByKeyed, typeof(EFRepository_CommonOrder), Enum_UserType.Agent)]
    public class EFRepository_AgentOrder : EFRepository_CommonOrder<Ent.TE_AgentOrder, long>
    {
        //
    }
}

