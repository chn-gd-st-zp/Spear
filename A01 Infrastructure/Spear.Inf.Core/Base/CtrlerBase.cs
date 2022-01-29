using System;

using Microsoft.AspNetCore.Mvc;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.Base
{
    public class CtrlerBase : ControllerBase
    {
        protected ISpearLogger Logger { get; private set; }

        public CtrlerBase()
        {
            Logger = ServiceContext.Resolve<ISpearLogger>();
        }
    }

    public class CtrlerBase<TLoggerType> : CtrlerBase
        where TLoggerType : class
    {
        protected new ISpearLogger<TLoggerType> Logger { get; private set; }

        public CtrlerBase() : base()
        {
            Logger = ServiceContext.Resolve<ISpearLogger<TLoggerType>>();
        }
    }

    public class CtrlerBase<TLoggerType, TCache> : CtrlerBase<TLoggerType>
        where TLoggerType : class
        where TCache : ICache
    {
        protected TCache Cache { get; private set; }

        public CtrlerBase() : base()
        {
            Cache = ServiceContext.Resolve<TCache>();
        }
    }

    public static class CtrlorBaseExtend
    {
        public static ResultWebApi<TResult> ToAPIResult<TResult>(this ResultBase<TResult> result)
        {
            return result.ToResultWebApi();
        }
    }
}
