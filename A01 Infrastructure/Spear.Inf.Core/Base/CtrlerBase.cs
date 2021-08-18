using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core.Base
{
    public class CtrlerBase : ControllerBase
    {
        protected new HttpContext HttpContext { get; private set; }
        protected ILogger Logger { get; private set; }

        public CtrlerBase()
        {
            HttpContext = Resolve<IHttpContextAccessor>().HttpContext;
            Logger = Resolve<ILogger>();
        }

        protected TTarget Resolve<TTarget>()
        {
            return ServiceContext.Resolve<TTarget>();
        }
    }

    public class CtrlerBase<TLoggerType> : CtrlerBase
        where TLoggerType : class
    {
        protected new ILogger<TLoggerType> Logger { get; private set; }

        public CtrlerBase() : base()
        {
            Logger = Resolve<ILogger<TLoggerType>>();
        }
    }

    public class CtrlerBase<TLoggerType, TCache> : CtrlerBase<TLoggerType>
        where TLoggerType : class
        where TCache : ICache
    {
        protected TCache Cache { get; private set; }

        public CtrlerBase() : base()
        {
            Cache = Resolve<TCache>();
        }
    }
}
