using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core.Base
{
    public class CtrlerBase : ControllerBase
    {
        protected new HttpContext HttpContext { get; private set; }
        protected ISpearLogger Logger { get; private set; }

        public CtrlerBase()
        {
            HttpContext = Resolve<IHttpContextAccessor>().HttpContext;
            Logger = Resolve<ISpearLogger>();
        }

        protected TTarget Resolve<TTarget>()
        {
            return ServiceContext.Resolve<TTarget>();
        }
    }

    public class CtrlerBase<TLoggerType> : CtrlerBase
        where TLoggerType : class
    {
        protected new ISpearLogger<TLoggerType> Logger { get; private set; }

        public CtrlerBase() : base()
        {
            Logger = Resolve<ISpearLogger<TLoggerType>>();
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
