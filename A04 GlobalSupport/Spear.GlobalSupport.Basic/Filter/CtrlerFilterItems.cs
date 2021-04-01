using System.Collections.Generic;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

namespace Spear.GlobalSupport.Basic.Filter
{
    [DIModeForService(Enum_DIType.SpecificByKeyed, typeof(IRequestFilterItems), Enum_FilterType.Ctrler)]
    public class CtrlerFilterItems : List<IRequestFilterItem>, IRequestFilterItems
    {
        public CtrlerFilterItems()
        {
            Add(new CtrlerFilterItemProcedure());
        }
    }
}
