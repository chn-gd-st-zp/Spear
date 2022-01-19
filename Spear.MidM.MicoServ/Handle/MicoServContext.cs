using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.MidM.MicoServ
{
    public class MicoServContext
    {
        public static TContainer Resolve<TContainer>(Enum_RegisCenter eRegisCenter, string serverIdentity) where TContainer : IMicoServContainer
        {
            var micoServConnector = ServiceContext.ResolveByKeyed<IMicoServConnector>(eRegisCenter);
            if (micoServConnector == null)
                return default;

            var micoServProvider = ServiceContext.Resolve<IMicoServProvider>();
            if (micoServProvider == null)
                return default;

            string servAddress = micoServConnector.GenericServAddress(serverIdentity);

            return micoServProvider.Resolve<TContainer>(servAddress);
        }
    }
}
