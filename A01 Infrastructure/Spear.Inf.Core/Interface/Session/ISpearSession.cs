using Spear.Inf.Core.ServGeneric.IOC;

namespace Spear.Inf.Core.Interface
{
    public interface ISpearSession : ITransient
    {
        /// <summary>
        /// 当前Token(字符串)
        /// </summary>
        string CurrentToken { get; }

        /// <summary>
        /// 当前会话对象
        /// </summary>
        SpearSessionInfo CurrentAccount { get; }

        void Set(SpearSessionInfo info);

        SpearSessionInfo Get(string accessToken);

        void Remove(string accessToken);

        void VerifyPermission(string permissionCode);
    }

    public interface ISpearSession<TTokenProvider> : ISpearSession where TTokenProvider : ITokenProvider
    {
        TTokenProvider TokenProvider { get; }
    }
}
