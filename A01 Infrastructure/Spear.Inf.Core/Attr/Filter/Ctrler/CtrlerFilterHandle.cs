using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core.Attr
{
    [DIModeForService(Enum_DIType.SpecificByKeyed, typeof(IRequestFilterHandle), Enum_FilterType.Ctrler)]
    public class CtrlerFilterHandle : IRequestFilterHandle
    {
        public IRequestFilterItems FilterItems { get { return _filterItems; } }
        private IRequestFilterItems _filterItems;

        public CtrlerFilterHandle()
        {
            _filterItems = ServiceContext.ResolveServByKeyed<IRequestFilterItems>(Enum_FilterType.Ctrler);
        }
    }
}
