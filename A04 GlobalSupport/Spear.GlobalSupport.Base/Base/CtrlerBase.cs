using Spear.Inf.Core.ServGeneric;
using Spear.MidM.SessionNAuth;

namespace Spear.GlobalSupport.Base
{
    public class CtrlerBase<T> : Inf.Core.Base.CtrlerBase<T> where T : class
    {
        protected ISessionNAuth<HTTPTokenProvider> SessionNAuth { get; set; }

        public CtrlerBase()
        {
            SessionNAuth = ServiceContext.Resolve<ISessionNAuth<HTTPTokenProvider>>();
        }
    }
}
