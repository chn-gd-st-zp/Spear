using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core.Attr
{
    [DIModeForService(Enum_DIType.SpecificByKeyed, typeof(IRequestFilterHandle), Enum_FilterType.GRPC)]
    public class GRPCFilterHandle : IRequestFilterHandle
    {
        public IRequestFilterItems FilterItems { get { return _filterItems; } }
        private IRequestFilterItems _filterItems;

        public GRPCFilterHandle()
        {
            _filterItems = ServiceContext.ResolveByKeyed<IRequestFilterItems>(Enum_FilterType.GRPC);
        }
    }
}
