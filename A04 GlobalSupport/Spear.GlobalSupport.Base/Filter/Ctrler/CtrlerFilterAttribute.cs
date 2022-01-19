using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Filters;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

using ServiceContext = Spear.Inf.Core.ServGeneric.ServiceContext;

namespace Spear.GlobalSupport.Base.Filter
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class CtrlerFilterAttribute : ActionFilterAttribute, IExceptionFilter, IAsyncExceptionFilter
    {
        private IRequestFilterHandle _filters;

        public CtrlerFilterAttribute()
        {
            _filters = ServiceContext.ResolveByKeyed<IRequestFilterHandle>(Enum_FilterType.Ctrler);
        }

        /// <summary>
        /// 执行顺序：1
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return base.OnActionExecutionAsync(context, next);
        }

        /// <summary>
        /// 执行顺序：2
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            foreach (var filter in _filters.FilterItems)
                filter.OnExecuting(context);
        }

        /// <summary>
        /// 执行顺序：3
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            foreach (var filter in _filters.FilterItems)
                filter.OnExecuted(context);
        }

        /// <summary>
        /// 执行顺序：4
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            return base.OnResultExecutionAsync(context, next);
        }

        /// <summary>
        /// 执行顺序：5
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);

            foreach (var filter in _filters.FilterItems)
                filter.OnExit(context);
        }

        /// <summary>
        /// 执行顺序：6
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }

        /// <summary>
        /// 执行顺序：4
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual Task OnExceptionAsync(ExceptionContext context)
        {
            foreach (var filter in _filters.FilterItems)
                filter.OnException(context, context.Exception);

            foreach (var filter in _filters.FilterItems)
                filter.OnExit(context);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 执行顺序：4
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnException(ExceptionContext context)
        {
            foreach (var filter in _filters.FilterItems)
                filter.OnException(context, context.Exception);

            foreach (var filter in _filters.FilterItems)
                filter.OnExit(context);
        }
    }
}
