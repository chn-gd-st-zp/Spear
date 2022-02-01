using System;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Spear.Inf.Core;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.Base;
using Spear.Inf.Core.CusException;
using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;

namespace Spear.GlobalSupport.Base.Filter
{
    public abstract class CtrlerFilterItemBase : IRequestFilterItem
    {
        protected readonly ISpearLogger Logger;

        public CtrlerFilterItemBase() { Logger = ServiceContext.Resolve<ISpearLogger>(); }

        public string Entrance { get; set; }
        public string Action { get; set; }
        public object Header { get; set; }
        public object ReqParams { get; set; }
        public object FuncParams { get; set; }
        public object Result { get; set; }
        public Exception Exception { get; set; }
        public DateTime? RequestTime { get; set; }
        public DateTime? ResponseTime { get; set; }

        public virtual void OnExecuting(object context)
        {
            var realContext = context as ActionExecutingContext;

            RequestTime = DateTime.Now;

            var paramDic = realContext.ActionArguments.ToDictionary(k => k.Key, v => v.Value);

            Entrance = realContext.RouteData.Values["controller"].ToString();
            Action = realContext.RouteData.Values["action"].ToString();
            Header = realContext.HttpContext.Request.Headers.ToDictionary(k => k.Key, v => v.Value as object);
            ReqParams = realContext.HttpContext.Request.GetRequestValue().Result;
            FuncParams = paramDic;

            #region 参数验证

            //微软的参数验证
            if (
                1 == 1
                && paramDic != null
                && paramDic.Count() != 0
                && paramDic.Values.Select(o => o.GetType().IsExtendOf<IDTO_Input>()).Any(o => o)
                && realContext.ModelState.IsValid == false
            )
            {
                var errorMsg = realContext.ModelState.GetValidationSummary();
                throw new Exception_ParamsValidationFailed(errorMsg);
            }

            //自定义的参数验证
            paramDic.Select(o => o.Value).ToArray().Verify();

            #endregion
        }

        public virtual void OnExecuted(object context)
        {
            var realContext = context as ActionExecutedContext;

            ResponseTime = DateTime.Now;

            //没有异常才处理Result，如果有异常Result会被异常代替
            if (realContext.Exception == null)
            {
                //如果标记[LogIgnore]，则不做日志记录
                if (((ControllerActionDescriptor)realContext.ActionDescriptor).MethodInfo.GetCustomAttribute<LogIgnoreAttribute>() == null)
                {
                    Result = realContext.Result;

                    if (realContext.Result is FileResult)
                    {
                        var result_tmp = realContext.Result as FileResult;
                        Result = new ContentResult() { Content = result_tmp.GetType().FullName, ContentType = result_tmp.ContentType };
                    }

                    //记录每次请求的往返内容
                    Logger.Info(new
                    {
                        Entrance,
                        Action,
                        Header,
                        ReqParams,
                        FuncParams,
                        Result,
                        RequestTime,
                        ResponseTime,
                    });
                }
            }
        }

        public virtual void OnExit(object context)
        {
            if (context is ResultExecutingContext)
            {
                var realContext = context as ResultExecutingContext;
                realContext.Result = Result as IActionResult;
                return;
            }

            if (context is ExceptionContext)
            {
                var realContext = context as ExceptionContext;
                realContext.Result = Result.ToJsonResult();
                return;
            }
        }

        public virtual void OnException(object context, Exception exception)
        {
            var realContext = context as ExceptionContext;

            //获取最底层的错误
            Exception = exception;
            while (Exception != null && Exception.InnerException != null)
                Exception = Exception.InnerException;

            Result = new ProcessResult<bool> { IsSuccess = false, ExInfo = Exception }.ToAPIResult();

            //是否标注了 志记录忽略 的标签，无标注 则需进行 日志记录
            if (Exception.GetType().GetCustomAttribute<LogIgnoreAttribute>() == null)
            {
                //记录错误日志
                Logger.Error(new
                {
                    Entrance,
                    Action,
                    Header,
                    ReqParams,
                    FuncParams,
                    Result,
                    RequestTime,
                    ResponseTime,
                }, Exception);
            }

            realContext.ExceptionHandled = true;
        }
    }

    public static class CtrlerFilterItemExtend
    {
        /// <summary>
        /// 获取验证消息提示并格式化提示
        /// </summary>
        public static string GetValidationSummary(this ModelStateDictionary modelState, string separator = "\r\n")
        {
            if (modelState.IsValid) return null;

            var error = new StringBuilder();

            foreach (var item in modelState)
            {
                var state = item.Value;
                var message = state.Errors.FirstOrDefault(p => !p.ErrorMessage.IsEmptyString())?.ErrorMessage;

                if (message.IsEmptyString())
                    message = state.Errors.FirstOrDefault(o => o.Exception != null)?.Exception.Message;

                if (message.IsEmptyString())
                    continue;

                if (error.Length > 0)
                    error.Append(separator);

                error.Append(message);
            }

            return error.ToString();
        }
    }
}
