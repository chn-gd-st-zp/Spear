using System.Collections.Generic;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

namespace Spear.GlobalSupport.Base.Filter
{
    [DIModeForService(Enum_DIType.SpecificByKeyed, typeof(IRequestFilterItems), Enum_FilterType.GRPC)]
    public class GRPCFilterItems : List<IRequestFilterItem>, IRequestFilterItems
    {
        public GRPCFilterItems()
        {
            Add(new GRPCFilterItemForAuth());
        }
    }
}
