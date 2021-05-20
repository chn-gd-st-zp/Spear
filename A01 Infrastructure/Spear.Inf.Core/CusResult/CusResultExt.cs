using System;

using MagicOnion;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.CusException;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.CusResult
{
    public static class CusResultExtend
    {
        #region ResultBasic

        public static ResultBasic<T> ResultBasic_Success<T>(this T data, string msg = "操作成功")
        {
            return new ResultBasic<T>(data, msg);
        }

        public static ResultBasic<T> ResultBasic_Fail<T>(this T data, string msg = "操作失败")
        {
            return new ResultBasic<T>(msg);
        }

        public static ResultBasic<T> ResultBasic_Fail<T>(this string msg)
        {
            return new ResultBasic<T>(msg);
        }

        public static ResultBasic<T> ResultBasic_Exception<T>(this Exception exception, string msg = "操作失败")
        {
            return new ResultBasic<T>(exception, msg);
        }

        #endregion

        #region ResultAPI

        public static ResultWebApi<T> ToResultWebApi<T>(this ResultBasic<T> resultBasic)
        {
            if (resultBasic.IsSuccess)
                return resultBasic.Data.Success(resultBasic.Msg);
            else if (resultBasic.ExInfo == null)
                return resultBasic.Data.Fail(resultBasic.Msg);
            else
                return resultBasic.Data.Exception(resultBasic.ExInfo);
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

        public static ResultWebApi<T> Success<T>(this T data, string msg = "操作成功")
        {
            return data.ToResultWebApi(Enum_StateCode.Success.ToIntString(), msg);
        }

        public static ResultWebApi<T> Fail<T>(this T data, string code = "", string msg = "操作失败")
        {
            return data.ToResultWebApi(code, msg);
        }

        public static ResultWebApi<T> Fail<T>(this T data, string msg)
        {
            return data.Fail(Enum_StateCode.Fail.ToIntString(), msg);
        }

        public static ResultWebApi<T> Fail<T>(this T data)
        {
            return data.Fail(Enum_StateCode.Fail.ToIntString());
        }

        public static ResultWebApi<T> Exception<T>(this T data, Exception exception)
        {
            EnumInfo errorCode = null;
            string errorMsg = "";

            if (exception.IsExtendType<Exception_Basic>())
            {
                var e = exception as Exception_Basic;

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

        public static ResultMicServ<T> ToResultMicServ<T>(this ResultBasic<T> resultBasic)
        {
            ResultMicServ<T> result;

            result = new ResultMicServ<T>();
            result.IsSuccess = resultBasic.IsSuccess;
            result.Code = Enum_StateCode.Success.ToIntString();
            result.Msg = resultBasic.Msg;
            result.Data = resultBasic.Data;

            if (resultBasic.ExInfo != null)
            {
                Exception_Basic cusEx = resultBasic.ExInfo as Exception_Basic;
                result.Code = cusEx != null ? cusEx.ECode.ToIntString() : Enum_StateCode.SysError.ToIntString();
                result.Msg = resultBasic.ExInfo.Message;
                result.ErrorStackTrace += "\r\n" + resultBasic.ExInfo.StackTrace;
            }

            return result;
        }

        public static UnaryResult<ResultMicServ<T>> ToMicServResult<T>(this ResultBasic<T> resultBasic)
        {
            return new UnaryResult<ResultMicServ<T>>(resultBasic.ToResultMicServ());
        }

        #endregion
    }
}
