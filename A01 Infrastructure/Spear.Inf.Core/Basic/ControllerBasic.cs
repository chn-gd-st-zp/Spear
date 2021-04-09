using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core.Basic
{
    public class ControllerBasic : ControllerBase
    {
        protected new HttpContext HttpContext { get; private set; }
        protected ICache4Redis Cache { get; private set; }
        protected ILogger Logger { get; private set; }

        public ControllerBasic()
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
