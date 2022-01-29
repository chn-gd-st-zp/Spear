using System;
using Spear.Inf.Core.Base;
using Spear.Inf.Core.CusException;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.CusResult
{
    public static class CusResultExtend
    {
        #region ResultBase

        internal static ResultBase<T> ResultBase_Success<T>(this T data, string msg = "操作成功")
        {
            return new ResultBase<T>(data, msg);
        }

        internal static ResultBase<T> ResultBase_Fail<T>(this T data, string msg = "操作失败")
        {
            return new ResultBase<T>(msg);
        }

        internal static ResultBase<T> ResultBase_Fail<T>(this string msg)
        {
            return new ResultBase<T>(msg);
        }

        internal static ResultBase<T> ResultBase_Exception<T>(this Exception exception, string msg = "操作失败")
        {
            return new ResultBase<T>(exception, msg);
        }

        #endregion

        #region ResultAPI

        internal static ResultWebApi<T> ToResultWebApi<T>(this ResultBase<T> resultBase)
        {
            if (resultBase.IsSuccess)
                return resultBase.Data.ResultWebApi_Success(resultBase.Msg);
            else if (resultBase.ExInfo == null)
                return resultBase.Data.ResultWebApi_Fail(resultBase.Msg);
            else
                return resultBase.Data.ResultWebApi_Exception(resultBase.ExInfo);
        }

        internal static ResultWebApi<T> ToResultWebApi<T>(this T data, string code, string msg)
        {
            ResultWebApi<T> result;

            result = new ResultWebApi<T>();
            result.IsSuccess = true;
            result.Code = code;
            result.Msg = msg;
            result.Data = data;

            return result;
        }

        internal static ResultWebApi<T> ResultWebApi_Success<T>(this T data, string msg = "操作成功")
        {
            return data.ToResultWebApi(Enum_StateCode.Success.ToIntString(), msg);
        }

        internal static ResultWebApi<T> ResultWebApi_Fail<T>(this T data, string code = "", string msg = "操作失败")
        {
            return data.ToResultWebApi(code, msg);
        }

        internal static ResultWebApi<T> ResultWebApi_Fail<T>(this T data, string msg)
        {
            return data.ResultWebApi_Fail(Enum_StateCode.Fail.ToIntString(), msg);
        }

        internal static ResultWebApi<T> ResultWebApi_Fail<T>(this T data)
        {
            return data.ResultWebApi_Fail(Enum_StateCode.Fail.ToIntString());
        }

        internal static ResultWebApi<T> ResultWebApi_Exception<T>(this T data, Exception exception)
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
                errorMsg = AppInitHelper.IsTestMode ? exception.Message : "程序出现错误，请联系管理员";
#endif
            }

            ResultWebApi<T> result = ToResultWebApi<T>(default(T), errorCode.ToIntString(), errorMsg);

            return result;
        }

        #endregion
    }
}
