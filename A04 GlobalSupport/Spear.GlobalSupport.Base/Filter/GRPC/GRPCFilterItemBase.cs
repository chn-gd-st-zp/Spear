using System;
using System.Linq;
using System.Reflection;

using MessagePack;

using Spear.Inf.Core;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;
using Spear.MidM.MicoServ.MagicOnion;

using MS = MagicOnion.Server;

namespace Spear.GlobalSupport.Base.Filter
{
    public abstract class GRPCFilterItemBase : IRequestFilterItem
    {
        protected readonly ISpearLogger Logger;

        public GRPCFilterItemBase() { Logger = ServiceContext.Resolve<ISpearLogger>(); }

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
            MS.ServiceContext realContext = context as MS.ServiceContext;

            RequestTime = DateTime.Now;

            var paramArray = realContext.RestoreParams();

            Entrance = realContext.ServiceType.Name;
            Action = realContext.MethodInfo.Name;
            Header = realContext.CallContext.RequestHeaders.AsEnumerable().ToList().ToDictionary(k => k.Key, v => v.Value as object);
            ReqParams = paramArray;
            FuncParams = paramArray;

            #region 参数验证

            //自定义的参数验证
            paramArray.Verify();

            #endregion
        }

        public virtual void OnExecuted(object context)
        {
            MS.ServiceContext realContext = context as MS.ServiceContext;

            ResponseTime = DateTime.Now;

            //没有异常才处理Result，如果有异常Result会被异常代替
            if (Exception == null)
            {
                //如果标记[LogIgnore]，则不做日志记录
                if (realContext.MethodInfo.GetCustomAttribute<LogIgnoreAttribute>() == null)
                {
                    var respContent = realContext.GetRawResponse();

                    Result = respContent == null || respContent.Length == 0 ? new MagicOnionResult<object>() : MessagePackSerializer.Deserialize<MagicOnionResult<object>>(respContent.AsMemory());

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
                    }.ToJson());
                }
            }
        }

        public virtual void OnExit(object context)
        {
            MS.ServiceContext realContext = context as MS.ServiceContext;

            realContext.SetRawResponse(MessagePackSerializer.Serialize(Result));
        }

        public virtual void OnException(object context, Exception exception)
        {
            MS.ServiceContext realContext = context as MS.ServiceContext;

            //获取最底层的错误
            Exception = exception;
            while (Exception != null && Exception.InnerException != null)
                Exception = Exception.InnerException;

            Type resultBaseType = typeof(ResultBase<>);
            Type ResultMicoServType = realContext.MethodInfo.ReturnParameter.ParameterType.GenericTypeArguments[0];
            Type dataType = ResultMicoServType.GenericTypeArguments[0];
            MethodInfo func_ResultBase_Exception = typeof(CusResultExtend).GetMethod("ResultBase_Exception").MakeGenericMethod(dataType);
            MethodInfo func_ToResultMicoServ = typeof(MagicOnionResultExtend).GetMethod("ToMagicOnionResult").MakeGenericMethod(dataType);

            var resultBase = func_ResultBase_Exception.Invoke(null, new object[] { Exception, null });
            Result = func_ToResultMicoServ.Invoke(null, new object[] { resultBase });

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
                    ResponseTime
                }, Exception);
            }
        }
    }
}
