using Spear.Inf.Core.CusResult;

namespace Spear.GlobalSupport.Base
{
    public static class CtrlorBaseExtend
    {
        public static ResultWebApi<TResult> ToAPIResult<TResult>(this ResultBase<TResult> result)
        {
            return result.ToResultWebApi();
        }
    }

    public static class ServiceBaseExtend
    {
        public static ResultBase<TResult> ToServSuccess<TResult>(this TResult result, string msg = "")
        {
            return new ResultBase<TResult>(result, msg);
        }

        public static ResultBase<string> ToServFail(this string msg)
        {
            return new ResultBase<string>(msg);
        }

        public static ResultBase<TResult> ToServFail<TResult>(this string msg)
        {
            return new ResultBase<TResult>(msg);
        }

        public static ResultBase<TResult> ToServFail<TResult>(this TResult result, string msg = "")
        {
            return new ResultBase<TResult>(msg);
        }
    }
}
