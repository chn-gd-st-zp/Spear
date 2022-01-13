using System.Collections.Generic;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

using MS = MagicOnion.Server;

namespace Spear.GlobalSupport.Base.Filter
{
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(IRequestFilterItems), Enum_FilterType.GRPC)]
    public class GRPCFilterItems : List<IRequestFilterItem>, IRequestFilterItems
    {
        public GRPCFilterItems()
        {
            Add(new GRPCFilterItem());
        }
    }

    public class GRPCFilterItem : GRPCFilterItemBase
    {
        public override void OnExecuting(object context)
        {
            MS.ServiceContext realContext = context as MS.ServiceContext;

            base.OnExecuting(realContext);
        }
    }
}
