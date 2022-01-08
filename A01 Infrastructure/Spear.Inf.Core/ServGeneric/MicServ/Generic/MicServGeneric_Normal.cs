﻿using Grpc.Net.Client;
using MagicOnion.Client;

namespace Spear.Inf.Core.ServGeneric.MicServ
{
    public class MicServGeneric_Normal : IMicServGeneric
    {
        public readonly MicServClientSettings micServClientSettings;

        public MicServGeneric_Normal()
        {
            micServClientSettings = ServiceContext.Resolve<MicServClientSettings>();
        }

        public T GetServ<T>(params object[] paramArray) where T : IMicServ<T>
        {
            string address = paramArray == null || paramArray.Length == 0 ? micServClientSettings.Address : paramArray[0].ToString();

            GrpcChannel channel = GrpcChannel.ForAddress(address);
            var target = MagicOnionClient.Create<T>(channel);
            return target;
        }
    }
}