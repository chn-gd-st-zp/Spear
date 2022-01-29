using MagicOnion;
using MessagePack;

using Spear.Inf.Core;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.CusException;
using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.MicoServ.MagicOnion
{
    [MessagePackObject(true)]
    public class MagicOnionResult<TData> : MicoServResult<TData> { }

    public static class MagicOnionResultExtend
    {
        public static UnaryResult<MagicOnionResult<T>> ToUnaryResult<T>(this ResultBase<T> resultBase)
        {
            return new UnaryResult<MagicOnionResult<T>>(resultBase.ToMagicOnionResult());
        }

        public static MagicOnionResult<T> ToMagicOnionResult<T>(this ResultBase<T> resultBase)
        {
            MagicOnionResult<T> result;

            var stateCode = ServiceContext.Resolve<IStateCode>();

            result = new MagicOnionResult<T>();
            result.IsSuccess = resultBase.IsSuccess;
            result.Code = resultBase.IsSuccess ? stateCode.Success.ToIntString() : stateCode.Fail.ToIntString();
            result.Msg = resultBase.Msg;
            result.Data = resultBase.Data;

            if (resultBase.ExInfo != null)
            {
                var exception = resultBase.ExInfo;

                SpearEnumItem errorCode = null;
                string errorMsg = "";

                if (exception.IsExtendType<Exception_Base>())
                {
                    var e = resultBase.ExInfo as Exception_Base;

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

                var cusEx = resultBase.ExInfo as Exception_Base;
                result.Code = errorCode.ToIntString();
                result.Msg = errorMsg;
                result.ErrorStackTrace += "\r\n" + resultBase.ExInfo.StackTrace;
            }

            return result;
        }
    }
}
