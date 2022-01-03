﻿using System;
using System.Threading;

using MessagePack;
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

        public ResultBase(string msg)
        {
            IsSuccess = false;
            Msg = msg;
            Data = default;
            ExInfo = null;
        }

        public ResultBase(Exception exInfo, string msg)
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

    [MessagePackObject(true)]
    public class ResultMicServ<TData>
    {
        public bool IsSuccess { get; set; }

        public string Code { get; set; }

        public string Msg { get; set; }

        public TData Data { get; set; }

        public string ErrorStackTrace { get; set; }
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
