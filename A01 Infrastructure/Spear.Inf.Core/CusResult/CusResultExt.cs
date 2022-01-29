using System;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.CusException;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.CusResult
{
    public static class CusResultExtend
    {
        private static IStateCode stateCode = ServiceContext.Resolve<IStateCode>();

        internal static ResultWebApi<T> ToResultWebApi<T>(this ResultBase<T> resultBase)
        {
            if (resultBase.IsSuccess)
                return resultBase.Data.ResultWebApi_Success(resultBase.Msg);
            else if (resultBase.ExInfo == null)
                return resultBase.Data.ResultWebApi_Fail(resultBase.Msg);
            else
                return resultBase.Data.ResultWebApi_Exception(resultBase.ExInfo);
        }

        private static ResultWebApi<T> ResultWebApi_Success<T>(this T data, string msg = "操作成功")
        {
            return data.ToResultWebApi(stateCode.Success, msg);
        }

        private static ResultWebApi<T> ResultWebApi_Fail<T>(this T data, string msg = "操作失败")
        {
            return data.ToResultWebApi(stateCode.Fail, msg);
        }

        private static ResultWebApi<T> ResultWebApi_Exception<T>(this T data, Exception exception)
        {
            SpearEnumItem errorCode = null;
            string errorMsg = "";

            if (exception.IsExtendType<Exception_Base>())
            {
                var e = exception as Exception_Base;

                errorCode = e.ECode;
                errorMsg = e.Message;
            }
            else
            {
                errorCode = stateCode.SysError;

#if DEBUG
                errorMsg = exception.Message;
#else
                errorMsg = AppInitHelper.IsTestMode ? exception.Message : "程序出现错误，请联系管理员";
#endif
            }

            ResultWebApi<T> result = ToResultWebApi(default(T), errorCode, errorMsg);

            return result;
        }

        private static ResultWebApi<T> ToResultWebApi<T>(this T data, SpearEnumItem code, string msg)
        {
            ResultWebApi<T> result;

            result = new ResultWebApi<T>();
            result.IsSuccess = true;
            result.Code = code.ToIntString();
            result.Msg = msg;
            result.Data = data;

            return result;
        }
    }
}
