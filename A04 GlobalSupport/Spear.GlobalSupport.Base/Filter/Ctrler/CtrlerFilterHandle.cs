using Spear.Inf.Core;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

namespace Spear.GlobalSupport.Base.Filter
{
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(IRequestFilterHandle), Enum_FilterType.Ctrler)]
    public class CtrlerFilterHandle : IRequestFilterHandle
    {
        public IRequestFilterItems FilterItems { get { return _filterItems; } }
        private IRequestFilterItems _filterItems;

        public CtrlerFilterHandle()
        {
            _filterItems = ServiceContext.ResolveByKeyed<IRequestFilterItems>(Enum_FilterType.Ctrler);
        }
    }
}
