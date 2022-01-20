using Spear.Inf.Core.ServGeneric.IOC;

namespace Spear.Inf.Core.Interface
{
    public interface ISpearSession : ITransient
    {
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
