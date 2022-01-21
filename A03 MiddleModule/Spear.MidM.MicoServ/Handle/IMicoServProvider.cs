using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Injection;

namespace Spear.MidM.MicoServ
{
    public interface IMicoServProvider : ISingleton
    {
        TContainer Resolve<TContainer>(string address) where TContainer : IMicoServContainer;
    }
}
