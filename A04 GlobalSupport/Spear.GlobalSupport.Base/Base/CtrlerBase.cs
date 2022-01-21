using Spear.Inf.Core;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

namespace Spear.GlobalSupport.Base
{
    public class CtrlerBase<T> : Inf.Core.Base.CtrlerBase<T> where T : class
    {
        protected ISpearSession SpearSession { get; set; }

        public CtrlerBase()
        {
            SpearSession = ServiceContext.ResolveByKeyed<ISpearSession>(Enum_Protocol.HTTP);
        }
    }
}
