using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Autofac;
using Consul;
using MagicOnion.Server;
using MessagePack;

using Spear.Inf.Core.DTO;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Tool;

using MS = MagicOnion.Server;

namespace Spear.Inf.Core.ServGeneric.MicServ
{
    public static class MicServExtend
    {
        public static void LoadMicServRunSettings(IConfigurationBuilder configBuilder, List<string> argList)
        {
            if (argList.Count() > 0)
            {
                string hostPublic = argList.Where(o => o.StartsWith("hostpublic=", StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                string hostInternal = argList.Where(o => o.StartsWith("hostinternal=", StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                string webApiPort = argList.Where(o => o.StartsWith("webapiport=", StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                string micServPort = argList.Where(o => o.StartsWith("micservport=", StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

                var settings = new MicServRunSettings()
                {
                    HostPublic = hostPublic.IsEmptyString() ? "" : hostPublic.Replace("hostpublic=", ""),
                    HostInternal = hostInternal.IsEmptyString() ? "" : hostInternal.Replace("hostinternal=", ""),
                    WebApiPort = webApiPort.IsEmptyString() ? 0 : int.Parse(webApiPort.Replace("webapiport=", "", StringComparison.OrdinalIgnoreCase)),
                    GRPCPort = micServPort.IsEmptyString() ? 0 : int.Parse(micServPort.Replace("micservport=", "", StringComparison.OrdinalIgnoreCase)),
                };

                var source = new Dictionary<string, string>
                {
                    ["MicServRunSettings:HostPublic"] = settings.HostPublic,
                    ["MicServRunSettings:HostInternal"] = settings.HostInternal,
                    ["MicServRunSettings:WebApiPort"] = settings.WebApiPort.ToString(),
                    ["MicServRunSettings:GRPCPort"] = settings.GRPCPort.ToString(),
                };

                configBuilder.AddInMemoryCollection(source);
            }
        }

        public static void AddFilter<T>(this IList<MagicOnionServiceFilterDescriptor> filters) where T : MagicOnionFilterAttribute
        {
            var obj = InstanceCreator.Create<T>();

            filters.Add(new MagicOnionServiceFilterDescriptor(obj));
        }

        public static ContainerBuilder RegisMicServGeneric(this ContainerBuilder containerBuilder, MicServClientSettings micServClientSettings)
        {
            switch (micServClientSettings.MSType)
            {
                case Enum_MSType.Normal:
                    containerBuilder.RegisterType<MicServGeneric_Normal>().As<IMicServGeneric>().InstancePerDependency();
                    break;
                case Enum_MSType.Consul:
                    containerBuilder.RegisterType<MicServGeneric_Consul>().As<IMicServGeneric>().InstancePerDependency();
                    break;
                default:
                    break;
            }

            return containerBuilder;
        }

        public static IEndpointRouteBuilder UseMagicOnion(this IEndpointRouteBuilder builder)
        {
            builder.MapMagicOnionService();

            //endpoints.MapMagicOnionHttpGateway("_", app.ApplicationServices.GetService<MagicOnion.Server.MagicOnionServiceDefinition>().MethodHandlers, GrpcChannel.ForAddress("https://localhost:1010"));
            //endpoints.MapMagicOnionSwagger("swagger", app.ApplicationServices.GetService<MagicOnion.Server.MagicOnionServiceDefinition>().MethodHandlers, "/_/");
            //endpoints.MapGet("/", async context => { await context.Response.WriteAsync(""); });

            return builder;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, MicServRunSettings micServRunSettings, MicServServerSettings micServServerSettings)
        {
            var registration = new AgentServiceRegistration()
            {
                ID = micServServerSettings.NodeName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Name = micServServerSettings.NodeName,
                Address = micServServerSettings.GetHostModes(Enum_ServType.GRPC, micServRunSettings).ToJson(),

                //健康监测
                Check = new AgentServiceCheck()
                {
                    //服务启动多久后注册
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),

                    //健康检查时间间隔，或者称为心跳间隔
                    Interval = TimeSpan.FromSeconds(10),

                    //超时设置
                    Timeout = TimeSpan.FromSeconds(10),

                    //健康检查地址
                    HTTP = micServServerSettings.GetServAddr_WebApi(micServRunSettings) + micServServerSettings.HealthCheckRoute,
                },

                //添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
                Tags = new[] { $"urlprefix-/{micServServerSettings.NodeName} proto=grpc grpcservername={micServServerSettings.NodeName}" },
            };

            //创建服务客户端
            var consulClient = new ConsulClient(o => o.Address = new Uri(micServServerSettings.ConsulAddress));

            //服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
            consulClient.Agent.ServiceRegister(registration).Wait();

            lifetime
                .ApplicationStopping
                .Register(() =>
                {
                    //服务停止时取消注册
                    consulClient.Agent.ServiceDeregister(registration.ID).Wait();
                });

            return app;
        }

        public static object[] RestoreParams(this MS.ServiceContext context)
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
    }
}
