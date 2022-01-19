using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Filters;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

namespace Spear.GlobalSupport.Base.Filter
{
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(IRequestFilterItems), Enum_FilterType.Ctrler)]
    public class CtrlerFilterItems : List<IRequestFilterItem>, IRequestFilterItems
    {
        public CtrlerFilterItems()
        {
            Add(new CtrlerFilterItem());
        }
    }

    public class CtrlerFilterItem : CtrlerFilterItemBase
    {
        public override void OnExecuting(object context)
        {
            ActionExecutingContext realContext = context as ActionExecutingContext;

            base.OnExecuting(realContext);
        }
    }
}
