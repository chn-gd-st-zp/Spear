using MagicOnion;
using MessagePack;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.CusException;
using Spear.Inf.Core.CusResult;

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

            result = new MagicOnionResult<T>();
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
    }
}
