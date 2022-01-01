using System;

using MagicOnion;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.CusException;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.CusResult
{
    public static class CusResultExtend
    {
        #region ResultBase

        public static ResultBase<T> ResultBase_Success<T>(this T data, string msg = "操作成功")
        {
            return new ResultBase<T>(data, msg);
        }

        public static ResultBase<T> ResultBase_Fail<T>(this T data, string msg = "操作失败")
        {
            return new ResultBase<T>(msg);
        }

        public static ResultBase<T> ResultBase_Fail<T>(this string msg)
        {
            return new ResultBase<T>(msg);
        }

        public static ResultBase<T> ResultBase_Exception<T>(this Exception exception, string msg = "操作失败")
        {
            return new ResultBase<T>(exception, msg);
        }

        #endregion

        #region ResultAPI

        public static ResultWebApi<T> ToResultWebApi<T>(this ResultBase<T> resultBase)
        {
            if (resultBase.IsSuccess)
                return resultBase.Data.ResultWebApi_Success(resultBase.Msg);
            else if (resultBase.ExInfo == null)
                return resultBase.Data.ResultWebApi_Fail(resultBase.Msg);
            else
                return resultBase.Data.ResultWebApi_Exception(resultBase.ExInfo);
        }

        public static ResultWebApi<T> ToResultWebApi<T>(this T data, string code, string msg)
        {
            ResultWebApi<T> result;

            result = new ResultWebApi<T>();
            result.Status = true;
            result.Code = int.Parse(code);
            result.Msg = msg;
            result.Data = data;

            return result;
        }

        public static ResultWebApi<T> ResultWebApi_Success<T>(this T data, string msg = "操作成功")
        {
            return data.ToResultWebApi(Enum_StateCode.Success.ToIntString(), msg);
        }

        public static ResultWebApi<T> ResultWebApi_Fail<T>(this T data, string code = "", string msg = "操作失败")
        {
            return data.ToResultWebApi(code, msg);
        }

        public static ResultWebApi<T> ResultWebApi_Fail<T>(this T data, string msg)
        {
            return data.ResultWebApi_Fail(Enum_StateCode.Fail.ToIntString(), msg);
        }

        public static ResultWebApi<T> ResultWebApi_Fail<T>(this T data)
        {
            return data.ResultWebApi_Fail(Enum_StateCode.Fail.ToIntString());
        }

        public static ResultWebApi<T> ResultWebApi_Exception<T>(this T data, Exception exception)
        {
            EnumInfo errorCode = null;
            string errorMsg = "";

            if (exception.IsExtendType<Exception_Base>())
            {
                var e = exception as Exception_Base;

                errorCode = e.ECode;
                errorMsg = e.Message;
            }
            else
            {
                errorCode = Enum_StateCode.SysError;
#if DEBUG
                errorMsg = exception.Message;
#else
                errorMsg = "程序出现错误，请联系管理员";
#endif
            }

            ResultWebApi<T> result = ToResultWebApi<T>(default(T), errorCode.ToIntString(), errorMsg);

            return result;
        }

        #endregion

        #region ResultMicServ

        public static ResultMicServ<T> ToResultMicServ<T>(this ResultBase<T> resultBase)
        {
            ResultMicServ<T> result;

            result = new ResultMicServ<T>();
            result.IsSuccess = resultBase.IsSuccess;
            result.Code = Enum_StateCode.Success.ToIntString();
            result.Msg = resultBase.Msg;
            result.Data = resultBase.Data;

            if (resultBase.ExInfo != null)
            {
                Exception_Base cusEx = resultBase.ExInfo as Exception_Base;
                result.Code = cusEx != null ? cusEx.ECode.ToIntString() : Enum_StateCode.SysError.ToIntString();
                result.Msg = resultBase.ExInfo.Message;
                result.ErrorStackTrace += "\r\n" + resultBase.ExInfo.StackTrace;
            }

            return result;
        }

        public static UnaryResult<ResultMicServ<T>> ToMicServResult<T>(this ResultBase<T> resultBase)
        {
            return new UnaryResult<ResultMicServ<T>>(resultBase.ToResultMicServ());
        }

        #endregion
    }
}
