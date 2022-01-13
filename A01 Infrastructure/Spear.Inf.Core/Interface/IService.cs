using Spear.Inf.Core.ServGeneric.IOC;

namespace Spear.Inf.Core.Interface
{
    public interface IService : ITransient
    {
        //
    }

    public interface IServiceWithTrigger<TService> : IService
        where TService : class, IService
    {
        //
    }

    public interface IServiceWithCache<TCache> : IService
        where TCache : ICache
    {
        //
    }

    public interface IServiceWithTokenProvider<TTokenProvider> : IService
        where TTokenProvider : ITokenProvider
    {
        //
    }

    public interface IService<TService> : IServiceWithTrigger<TService>
        where TService : class, IService
    {
        //
    }

    public interface IService<TService, TCache> : IService<TService>, IServiceWithTrigger<TService>, IServiceWithCache<TCache>
        where TService : class, IService
        where TCache : ICache
    {
        TCache Cache { get; }
    }

    public interface IService<TService, TCache, TTokenProvider> : IService<TService, TCache>, IServiceWithTrigger<TService>, IServiceWithCache<TCache>, IServiceWithTokenProvider<TTokenProvider>
        where TService : class, IService
        where TCache : ICache
        where TTokenProvider : ITokenProvider
    {
        ISession<TTokenProvider> Session { get; }
    }
}
