using MagicOnion;
using MessagePack;

using Spear.Inf.Core.AppEntrance;
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
        public static UnaryResult<MagicOnionResult<T>> ToUnaryResult<T>(this ProcessResult<T> processResult)
        {
            return new UnaryResult<MagicOnionResult<T>>(processResult.ToMagicOnionResult());
        }

        public static MagicOnionResult<T> ToMagicOnionResult<T>(this ProcessResult<T> processResult)
        {
            MagicOnionResult<T> result;

            var stateCode = ISpearEnum.Restore<IStateCode>();

            result = new MagicOnionResult<T>();
            result.IsSuccess = processResult.IsSuccess;
            result.Code = processResult.IsSuccess ? stateCode.Success : stateCode.Fail;
            result.Msg = processResult.Msg;
            result.Data = processResult.Data;

            if (processResult.ExInfo != null)
            {
                var exception = processResult.ExInfo;

                SpearEnumItem errorCode = null;
                string errorMsg = string.Empty;

                if (exception.GetType().IsExtendOf<Exception_Base>())
                {
                    var e = processResult.ExInfo as Exception_Base;

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

                var cusEx = processResult.ExInfo as Exception_Base;
                result.Code = errorCode;
                result.Msg = errorMsg;
                result.ErrorStackTrace += "\r\n" + processResult.ExInfo.StackTrace;
            }

            return result;
        }
    }
}
