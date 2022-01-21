using Spear.Inf.Core.Injection;

namespace Spear.Inf.Core.Interface
{
    public interface IMicoServContainer
    {
        //
    }

    public interface IMicoServConnector : ISingleton
    {
        string GenericServAddress(string serverIdentity);
    }
}
