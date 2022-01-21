using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.Base
{
    public class ServiceBase : IService
    {
        protected ISpearLogger Logger { get; private set; }

        public ServiceBase()
        {
            Logger = ServiceContext.Resolve<ISpearLogger>();
        }
    }

    public class ServiceBase<TService> : ServiceBase, IService<TService>
        where TService : class, IService
    {
        protected new ISpearLogger<TService> Logger { get; private set; }

        public ServiceBase()
        {
            Logger = ServiceContext.Resolve<ISpearLogger<TService>>();
        }
    }

    public class ServiceBase<TService, TCache> : ServiceBase<TService>, IService<TService, TCache>
        where TService : class, IService
        where TCache : ICache
    {
        public TCache Cache { get; private set; }

        public ServiceBase()
        {
            Cache = ServiceContext.Resolve<TCache>();
        }
    }

    public class ServiceBase<TService, TCache, TTokenProvider> : ServiceBase<TService, TCache>, IService<TService, TCache, TTokenProvider>
        where TService : class, IService
        where TCache : ICache
        where TTokenProvider : ITokenProvider
    {

        public ISpearSession<TTokenProvider> Session { get; private set; }

        public ServiceBase()
        {
            Session = ServiceContext.Resolve<ISpearSession<TTokenProvider>>();
        }
    }
}
