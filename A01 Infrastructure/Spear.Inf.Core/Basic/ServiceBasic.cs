using Microsoft.AspNetCore.Http;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core.Basic
{
    public class ServiceBasic
    {
        protected HttpContext HttpContext { get; private set; }
        protected ILogger Logger { get; private set; }

        public ServiceBasic()
        {
            HttpContext = Resolve<IHttpContextAccessor>().HttpContext;
            Logger = Resolve<ILogger>();
        }

        protected TTarget Resolve<TTarget>()
        {
            return ServiceContext.Resolve<TTarget>();
        }
    }

    public class ServiceBasic<TLoggerType> : ServiceBasic
        where TLoggerType : class
    {
        protected new ILogger<TLoggerType> Logger { get; private set; }

        public ServiceBasic() : base()
        {
            Logger = Resolve<ILogger<TLoggerType>>();
        }
    }

    public class ServiceBasic<TLoggerType, TCache> : ServiceBasic<TLoggerType>
        where TLoggerType : class
        where TCache : ICache
    {
        protected TCache Cache { get; private set; }

        public ServiceBasic() : base()
        {
            Cache = Resolve<TCache>();
        }
    }
}
