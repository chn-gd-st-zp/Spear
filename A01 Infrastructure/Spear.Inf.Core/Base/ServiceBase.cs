using Microsoft.AspNetCore.Http;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core.Base
{
    public class ServiceBase
    {
        protected HttpContext HttpContext { get; private set; }
        protected ISpearLogger Logger { get; private set; }

        public ServiceBase()
        {
            HttpContext = Resolve<IHttpContextAccessor>().HttpContext;
            Logger = Resolve<ISpearLogger>();
        }

        protected TTarget Resolve<TTarget>()
        {
            return ServiceContext.Resolve<TTarget>();
        }
    }

    public class ServiceBase<TLoggerType> : ServiceBase
        where TLoggerType : class
    {
        protected new ISpearLogger<TLoggerType> Logger { get; private set; }

        public ServiceBase() : base()
        {
            Logger = Resolve<ISpearLogger<TLoggerType>>();
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
