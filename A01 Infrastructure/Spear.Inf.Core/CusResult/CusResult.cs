using System;
using System.Threading;

using Newtonsoft.Json;

namespace Spear.Inf.Core.CusResult
{
    public class ResultBase<TData>
    {
        public bool IsSuccess { get; set; }

        public string Msg { get; set; }

        public TData Data { get; set; }

        public Exception ExInfo { get; set; }

        public ResultBase() { }

        public ResultBase(TData data, string msg = "")
        {
            IsSuccess = true;
            Msg = msg;
            Data = data;
            ExInfo = null;
        }

        public ResultBase(string msg = "操作失败")
        {
            IsSuccess = false;
            Msg = msg;
            Data = default;
            ExInfo = null;
        }

        public ResultBase(Exception exInfo, string msg = "运行异常")
        {
            IsSuccess = false;
            Msg = msg;
            Data = default;
            ExInfo = exInfo;
        }
    }

    public class ResultWebApi<TData>
    {
        [JsonIgnore]
        public bool IsSuccess { get; set; }

        public int Code { get; set; }

        public string Msg { get; set; }

        public TData Data { get; set; }
    }

    public class RespResultCallBack<T> : ResultBase<T>, IAsyncResult
    {
        public RespResultCallBack() { }

        #region 实现IAsyncResult

        public object AsyncState { get; protected set; }

        public WaitHandle AsyncWaitHandle { get; protected set; }

        public bool CompletedSynchronously { get; protected set; }

        public bool IsCompleted { get; protected set; }

        #endregion
    }
}
