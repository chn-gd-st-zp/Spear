using System;
using System.Linq;
using System.Reflection;

using MessagePack;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric.MicServ;
using Spear.Inf.Core.Tool;

using MS = MagicOnion.Server;
using ServiceContext = Spear.Inf.Core.ServGeneric.ServiceContext;

namespace Spear.Inf.Core.Attr
{
    public abstract class GRPCFilterItemBasic : IRequestFilterItem
    {
        protected readonly ILogger Logger;

        public GRPCFilterItemBasic() { Logger = ServiceContext.Resolve<ILogger>(); }

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
            FuncParams.Verify();

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

                    Result = respContent == null || respContent.Length == 0 ? new ResultMicServ<object>() : MessagePackSerializer.Deserialize<ResultMicServ<object>>(respContent.AsMemory());

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

            Type resultBasicType = typeof(ResultBasic<>);
            Type resultMicServType = realContext.MethodInfo.ReturnParameter.ParameterType.GenericTypeArguments[0];
            Type dataType = resultMicServType.GenericTypeArguments[0];
            Type funcClassType = typeof(CusResultExt);
            MethodInfo func_ResultBasic_Exception = funcClassType.GetMethod("ResultBasic_Exception").MakeGenericMethod(dataType);
            MethodInfo func_ToResultMicServ = funcClassType.GetMethod("ToResultMicServ").MakeGenericMethod(dataType);

            var resultBasic = func_ResultBasic_Exception.Invoke(null, new object[] { Exception, null });
            Result = func_ToResultMicServ.Invoke(null, new object[] { resultBasic });

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
