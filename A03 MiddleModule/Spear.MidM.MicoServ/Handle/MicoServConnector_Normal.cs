using Spear.Inf.Core;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

namespace Spear.MidM.MicoServ
{
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(IMicoServConnector), Enum_RegisCenter.Normal)]
    public class MicoServConnector_Normal : IMicoServConnector
    {
        public readonly MicoServClientSettings micoServClientSettings;

        public MicoServConnector_Normal()
        {
            micoServClientSettings = ServiceContext.Resolve<MicoServClientSettings>();
        }

        public string GenericServAddress(string serverIdentity)
        {
            var servAddress = serverIdentity;
            return servAddress;
        }
    }
}
