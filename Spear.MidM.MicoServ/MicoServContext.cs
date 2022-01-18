using System;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.MidM.MicoServ
{
    public class MicoServContext
    {
        public static void InitMicServClient()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        }

        public static TContainer Resolv<TContainer>(params object[] paramArray) where TContainer : IMicoServContainer<TContainer>
        {
            var micServGener = ServiceContext.Resolve<IMicoServProvider>();
            if (micServGener == null)
                return default;

            return micServGener.Resolve<TContainer>(paramArray);
        }
    }
}
