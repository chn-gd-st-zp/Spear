using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;

using Spear.Demo.Inf;
using Spear.Demo.DBIns.Stainless.Entity;

namespace Spear.Demo.DBIns.Stainless
{
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(EFRepository_CommonOrder), Enum_UserType.Customer)]
    public class EFRepository_CustomerOrder : EFRepository_CommonOrder<TB_CustomerOrder, string>
    {
        //
    }
}

