using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;

using MessagePack;
using MagicOnion.Server;

using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.MicoServ.MagicOnion
{
    public static class MagicOnionExtend
    {
        public static AppConfiguresBase MapMagicOnion(this AppConfiguresBase appConfigures)
        {
            appConfigures.App.UseEndpoints(endpoints =>
            {
                endpoints.MapMagicOnionService();
            });

            return appConfigures;
        }

        public static void AddFilter(this IList<MagicOnionServiceFilterDescriptor> filters, Type type)
        {
            filters.Add(new MagicOnionServiceFilterDescriptor(type));
        }

        public static void AddFilter(this IList<MagicOnionServiceFilterDescriptor> filters, params Type[] types)
        {
            foreach (var type in types)
                filters.Add(new MagicOnionServiceFilterDescriptor(type));
        }

        public static void AddFilter<T>(this IList<MagicOnionServiceFilterDescriptor> filters) where T : MagicOnionFilterAttribute
        {
            filters.Add(new MagicOnionServiceFilterDescriptor(typeof(T)));
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
    }
}
