using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

using Autofac;
using Grpc.Net.Client;
using MessagePack;
using MagicOnion.Client;
using MagicOnion.Server;

using Spear.Inf.Core.Tool;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Interface;

namespace Spear.MidM.MicoServ.MagicOnion
{
    public static class MagicOnionExtend
    {
        public static void AddFilter<T>(this IList<MagicOnionServiceFilterDescriptor> filters) where T : MagicOnionFilterAttribute
        {
            var obj = InstanceCreator.Create<T>();

            filters.Add(new MagicOnionServiceFilterDescriptor(obj));
        }

        public static object[] RestoreParams(this ServiceContext context)
        {
            Type inputType1 = typeof(IDTO_GRPC<>);
            Type inputType2 = typeof(IDTO_Input);

            var methodParamArray = context.MethodInfo.GetParameters().ToList();
            var reqParamArray = new List<object>();

            var reqContent = context.GetRawRequest();

            if (methodParamArray.Count() == 1)
            {
                if (reqContent != null && reqContent.Length != 0)
                    reqParamArray.Add(MessagePackSerializer.Deserialize<object>(reqContent.AsMemory()));
            }
            else if (methodParamArray.Count() > 1)
            {
                if (reqContent != null && reqContent.Length != 0)
                    reqParamArray.AddRange(MessagePackSerializer.Deserialize<object[]>(reqContent.AsMemory()));
            }

            if (methodParamArray.Count() != reqParamArray.Count())
                return reqParamArray.ToArray();

            for (int i = 0; i < methodParamArray.Count(); i++)
            {
                var mpType = methodParamArray[i].ParameterType;

                if (mpType.IsImplementedNExtendGenericType(inputType1))
                    reqParamArray[i] = reqParamArray[i].ToJson().ToObject(mpType);
                else if (mpType.IsExtendType(inputType2))
                    reqParamArray[i] = reqParamArray[i].ToJson().ToObject(mpType);
            }

            return reqParamArray.ToArray();
        }

        public static TContainer Resolve<TContainer>(string address) where TContainer : IMicoServContainer, IMagicOnionContainer<TContainer>
        {
            GrpcChannel channel = GrpcChannel.ForAddress(address);
            return MagicOnionClient.Create<TContainer>(channel);
        }

        public static ContainerBuilder RegisMagicOnion(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<MagicOnionProvider>().As<IMicoServProvider>().SingleInstance();

            return containerBuilder;
        }

        public static IEndpointRouteBuilder UseMagicOnion(this IEndpointRouteBuilder builder)
        {
            builder.MapMagicOnionService();

            return builder;
        }
    }
}
