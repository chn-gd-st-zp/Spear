using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core.Basic
{
    public class ControllerBasic : ControllerBase
    {
        protected new HttpContext HttpContext { get; private set; }
        protected ILogger Logger { get; private set; }

        public ControllerBasic()
        {
            HttpContext = Resolve<IHttpContextAccessor>().HttpContext;
            Logger = Resolve<ILogger>();
        }

        protected TTarget Resolve<TTarget>()
        {
            return ServiceContext.Resolve<TTarget>();
        }
    }

    public class ControllerBasic<TLoggerType> : ControllerBasic
        where TLoggerType : class
    {
        protected new ILogger<TLoggerType> Logger { get; private set; }

        public ControllerBasic() : base()
        {
            Logger = Resolve<ILogger<TLoggerType>>();
        }
    }

    public class ControllerBasic<TLoggerType, TCache> : ControllerBasic<TLoggerType>
        where TLoggerType : class
        where TCache : ICache
    {
        protected TCache Cache { get; private set; }

        public ControllerBasic() : base()
        {
            Cache = Resolve<TCache>();
        }
    }
}
