using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;

using Spear.Demo.Inf;
using Spear.Demo.DBIns.Stainless.Entity;

namespace Spear.Demo.DBIns.Stainless
{
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(EFRepository_CommonOrder), Enum_UserType.Agent)]
    public class EFRepository_AgentOrder : EFRepository_CommonOrder<TB_AgentOrder, long>
    {
        //
    }
}

