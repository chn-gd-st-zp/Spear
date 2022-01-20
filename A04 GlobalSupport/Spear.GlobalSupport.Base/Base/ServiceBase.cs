using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

using CoreSB = Spear.Inf.Core.Base;

namespace Spear.GlobalSupport.Base
{
    public class ServiceBase<TService, TCache, TTokenProvider> : CoreSB.ServiceBase<TService, TCache, TTokenProvider>
        where TCache : ICache
        where TService : class, IService
        where TTokenProvider : ITokenProvider
    {
        public ISpearSession<TTokenProvider> SessionNAuth { get; }

        public ServiceBase()
        {
            SessionNAuth = ServiceContext.Resolve<ISpearSession<TTokenProvider>>();
        }
    }
}
