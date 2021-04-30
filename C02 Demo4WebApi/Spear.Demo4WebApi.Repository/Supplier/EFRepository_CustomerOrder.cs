using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Demo4WebApi.Basic;

using Ent = Spear.DBIns.Stainless.Entity;

namespace Spear.Demo4WebApi.Repository
{
    [DIModeForService(Enum_DIType.SpecificByKeyed, typeof(EFRepository_CommonOrder), Enum_UserType.Customer)]
    public class EFRepository_CustomerOrder : EFRepository_CommonOrder<Ent.TE_CustomerOrder, string>
    {
        //
    }
}

