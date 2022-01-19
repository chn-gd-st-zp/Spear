using Spear.Inf.Core.ServGeneric.IOC;

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
