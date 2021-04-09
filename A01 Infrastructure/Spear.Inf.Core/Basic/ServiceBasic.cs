using Microsoft.AspNetCore.Http;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core.Basic
{
    public class ServiceBasic
    {
        protected HttpContext HttpContext { get; private set; }
        protected ICache4Redis Cache { get; private set; }
        protected ILogger Logger { get; private set; }

        public ServiceBasic()
        {
            HttpContext = Resolve<IHttpContextAccessor>().HttpContext;
            Cache = Resolve<ICache4Redis>();
            Logger = Resolve<ILogger>();
        }

        protected TTarget Resolve<TTarget>()
        {
            return ServiceContext.Resolve<TTarget>();
        }
    }
}
