using System;

using Newtonsoft.Json;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.CusException;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.CusResult
{
    public class WebApiResult<TData>
    {
        [JsonIgnore]
        public bool IsSuccess { get; set; }

        [JsonConverter(typeof(StateCodeJsonConverter<Enum_StateCode>))]
        public SpearEnumItem Code { get; set; }

        public string Msg { get; set; }

        public TData Data { get; set; }
    }

    public static class WebApiResultExtend
    {
        internal static WebApiResult<TData> ResultWebApi_Success<TData>(this TData data, string msg = "操作成功")
        {
            return data.ToResultWebApi(ISpearEnum.Restore<IStateCode>().Success, msg);
        }

        internal static WebApiResult<TData> ResultWebApi_Fail<TData>(this TData data, string msg = "操作失败")
        {
            return data.ToResultWebApi(ISpearEnum.Restore<IStateCode>().Fail, msg);
        }

        internal static WebApiResult<TData> ResultWebApi_Exception<TData>(this TData data, Exception exception)
        {
            SpearEnumItem errorCode = null;
            string errorMsg = string.Empty;

            if (exception.GetType().IsExtendOf<Exception_Base>())
            {
                var e = exception as Exception_Base;

                errorCode = e.ECode;
                errorMsg = e.Message;
            }
            else
            {
                errorCode = ISpearEnum.Restore<IStateCode>().SysError;

#if DEBUG
                errorMsg = exception.Message;
#else
                errorMsg = AppInitHelper.IsTestMode ? exception.Message : "程序出现错误，请联系管理员";
#endif
            }

            WebApiResult<TData> result = ToResultWebApi(default(TData), errorCode, errorMsg);

            return result;
        }

        private static WebApiResult<TData> ToResultWebApi<TData>(this TData data, SpearEnumItem code, string msg)
        {
            WebApiResult<TData> result;

            result = new WebApiResult<TData>();
            result.IsSuccess = true;
            result.Code = code;
            result.Msg = msg;
            result.Data = data;

            return result;
        }
    }
}
