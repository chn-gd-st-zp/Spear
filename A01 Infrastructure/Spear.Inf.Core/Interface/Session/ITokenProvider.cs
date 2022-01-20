using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.ServGeneric.IOC;

namespace Spear.Inf.Core.Interface
{
    public interface ITokenProvider : ITransient
    {
        Enum_Protocol Protocol { get; }

        string CurrentToken { get; }
    }
}
