using System;

using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.CusResult
{
    public class ProcessResult
    {
        public static ProcessResult<TData> FromException<TData>(Type type, Exception exception)
        {
            return InstanceCreator.CreateGenericType(typeof(ProcessResult<>), type, exception) as ProcessResult<TData>;
        }
    }

    public class ProcessResult<TData>
    {
        public bool IsSuccess { get; set; }

        public string Msg { get; set; }

        public TData Data { get; set; }

        public Exception ExInfo { get; set; }

        public ProcessResult() { }

        public ProcessResult(TData data, string msg = "")
        {
            IsSuccess = true;
            Msg = msg;
            Data = data;
            ExInfo = null;
        }

        public ProcessResult(string msg = "操作失败")
        {
            IsSuccess = false;
            Msg = msg;
            Data = default;
            ExInfo = null;
        }

        public ProcessResult(Exception exInfo)
        {
            IsSuccess = false;
            Msg = exInfo.Message;
            Data = default;
            ExInfo = exInfo;
        }
    }

    public static class ProcessResultExtend
    {
        internal static WebApiResult<T> ToResultWebApi<T>(this ProcessResult<T> processResult)
        {
            if (processResult.IsSuccess)
                return processResult.Data.ResultWebApi_Success(processResult.Msg);
            else if (processResult.ExInfo == null)
                return processResult.Data.ResultWebApi_Fail(processResult.Msg);
            else
                return processResult.Data.ResultWebApi_Exception(processResult.ExInfo);
        }
    }
}
