using System;
using System.Threading;

using MessagePack;
using Newtonsoft.Json;

namespace Spear.Inf.Core.CusResult
{
    public class ResultBasic<TData>
    {
        public bool IsSuccess { get; set; }
        public string Msg { get; set; }
        public TData Data { get; set; }
        public Exception ExInfo { get; set; }

        public ResultBasic() { }

        public ResultBasic(TData data, string msg = "")
        {
            IsSuccess = true;
            Msg = msg;

            Data = data;
            ExInfo = null;
        }

        public ResultBasic(string msg)
        {
            IsSuccess = false;
            Msg = msg;

            Data = default;
            ExInfo = null;
        }

        public ResultBasic(Exception exInfo, string msg)
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
        public bool Status { get; set; }

        public int Code { get; set; }

        public string Msg { get; set; }

        public TData Data { get; set; }
    }

    [MessagePackObject(true)]
    public class ResultMicServ<TData>
    {
        public bool IsSuccess { get; set; }
        public string Code { get; set; }
        public string Msg { get; set; }
        public TData Data { get; set; }
        public string ErrorStackTrace { get; set; }
    }

    public class RespResultCallBack<T> : ResultBasic<T>, IAsyncResult
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
