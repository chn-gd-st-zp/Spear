using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.ServGeneric.IOC;

namespace Spear.Inf.Core.Interface
{
    public interface ITokenProvider : IScoped
    {
        Enum_Protocol Protocol { get; }

        string CurrentToken { get; }
    }

    public interface ISession : IScoped { }

    public interface ISession<TTokenProvider> : ISession where TTokenProvider : ITokenProvider
    {
        TTokenProvider TokenProvider { get; }
    }
}
