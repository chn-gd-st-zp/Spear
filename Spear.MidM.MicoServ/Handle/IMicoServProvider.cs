using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric.IOC;

namespace Spear.MidM.MicoServ
{
    public interface IMicoServProvider : ISingleton
    {
        TContainer Resolve<TContainer>(string address) where TContainer : IMicoServContainer;
    }
}
