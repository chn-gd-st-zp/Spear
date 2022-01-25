using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Autofac;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.SettingsGeneric
{
    public static class SettingsExtend
    {
        /// <summary>
        /// 覆盖注入配置
        /// </summary>
        /// <typeparam name="TNewSettings"></typeparam>
        /// <typeparam name="TDistSettings"></typeparam>
        /// <param name="containerBuilder"></param>
        /// <param name="redisSettings"></param>
        /// <returns></returns>
        public static ContainerBuilder CoverSettings<TNewSettings, TDistSettings>(this ContainerBuilder containerBuilder, TNewSettings newSettings)
            where TNewSettings : class, TDistSettings
            where TDistSettings : class, ISettings
        {
            containerBuilder.Register(o => newSettings).As(typeof(TDistSettings)).SingleInstance();

            return containerBuilder;
        }

        /// <summary>
        /// 监控配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void Monitor<T>(this IServiceCollection services, IConfiguration configuration) where T : class, ISettings
        {
            services.Configure<T>(configuration);
        }

        /// <summary>
        /// 监听配置更新
        /// </summary>
        /// <param name="app"></param>
        /// <param name="typeList"></param>
        public static void MonitorSettings(this IApplicationBuilder app, IEnumerable<Type> typeList)
        {
            foreach (var classType in typeList.Where(o => o.IsClass && !o.IsAbstract).ToList())
            {
                try
                {
                    if (!classType.IsImplementedType<ISettings>())
                        continue;

                    typeof(SettingsExtend).GetMethod("SettingsOnChange")
                        .MakeGenericMethod(new[] { classType })
                        .Invoke(null, null);
                }
                catch
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 动态更新配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void SettingsOnChange<T>() where T : class
        {
            var objType = typeof(T);
            if (!objType.IsImplementedType<ISettings>())
                return;

            var optionsMonitor = ServiceContext.Resolve<IOptionsMonitor<T>>();
            optionsMonitor.OnChange((T, listener) =>
            {
                var attrList = objType.GetCustomAttributes<DIModeForSettingsAttribute>();
                if (attrList == null || attrList.Count() == 0)
                    return;

                var piArray = objType.GetProperties();
                var configuration = ServiceContext.Resolve<IConfiguration>();

                T obj_target = null;
                T obj_source = null;

                foreach (var attr in attrList)
                {
                    obj_source = configuration.GetSetting(attr.ConfigRootName, objType) as T;

                    switch (attr.EDIType)
                    {
                        case Enum_DIType.AsSelf:
                            obj_target = ServiceContext.Resolve<T>();
                            break;
                        case Enum_DIType.AsImpl:

                            Type interfaceType = null;
                            foreach (var interfaceItem in objType.GetInterfaces())
                            {
                                if (interfaceItem is ISettings || interfaceItem.IsImplementedType<ISettings>())
                                {
                                    interfaceType = interfaceItem;
                                    break;
                                }
                            }

                            if (interfaceType == null)
                                continue;

                            obj_target = ServiceContext.Resolve(interfaceType) as T;
                            break;
                        case Enum_DIType.Exclusive:
                            obj_target = ServiceContext.Resolve(attr.Type) as T;
                            break;
                        case Enum_DIType.ExclusiveByNamed:
                            obj_target = ServiceContext.ResolveByNamed(attr.Type, attr.Key.ToString()) as T;
                            break;
                        case Enum_DIType.ExclusiveByKeyed:
                            obj_target = ServiceContext.ResolveByKeyed(attr.Type, attr.Key.ToString()) as T;
                            break;
                    }

                    if (obj_target == null || obj_source == null)
                        continue;

                    foreach (var pi in piArray)
                        pi.SetValue(obj_target, pi.GetValue(obj_source));
                }
            });
        }
    }
}
