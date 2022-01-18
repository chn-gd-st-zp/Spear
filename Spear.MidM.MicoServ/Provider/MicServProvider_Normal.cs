using Grpc.Net.Client;
using MagicOnion.Client;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.MidM.MicoServ
{
    public class MicServProvider_Normal : IMicoServProvider
    {
        public readonly MicoServClientSettings micoServClientSettings;

        public MicServProvider_Normal()
        {
            micoServClientSettings = ServiceContext.Resolve<MicoServClientSettings>();
        }

        public TContainer Resolve<TContainer>(params object[] paramArray) where TContainer : IMicoServContainer
        {
            string address = paramArray == null || paramArray.Length == 0 ? micoServClientSettings.Address : paramArray[0].ToString();

            GrpcChannel channel = GrpcChannel.ForAddress(address);
            //var target = MagicOnionClient.Create<TContainer>(channel);
            //return target;

            return default(TContainer);
        }
    }
}
