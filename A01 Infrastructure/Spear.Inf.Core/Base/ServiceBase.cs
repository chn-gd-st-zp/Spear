using Microsoft.AspNetCore.Http;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core.Base
{
    public class ServiceBase
    {
        protected HttpContext HttpContext { get; private set; }
        protected ILogger Logger { get; private set; }

        public ServiceBase()
        {
            HttpContext = Resolve<IHttpContextAccessor>().HttpContext;
            Logger = Resolve<ILogger>();
        }

        protected TTarget Resolve<TTarget>()
        {
            return ServiceContext.Resolve<TTarget>();
        }
    }

    public class ServiceBase<TLoggerType> : ServiceBase
        where TLoggerType : class
    {
        protected new ILogger<TLoggerType> Logger { get; private set; }

        public ServiceBase() : base()
        {
            Logger = Resolve<ILogger<TLoggerType>>();
        }
    }

    public class ServiceBase<TLoggerType, TCache> : ServiceBase<TLoggerType>
        where TLoggerType : class
        where TCache : ICache
    {
        protected TCache Cache { get; private set; }

        public ServiceBase() : base()
        {
            Cache = Resolve<TCache>();
        }
    }
}
