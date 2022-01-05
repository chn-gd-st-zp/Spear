using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.Configuration;

using Autofac;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.SettingsGeneric;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.ServGeneric.IOC
{
    public static class AutoFacExtend
    {
        /// <summary>
        /// 注入配置
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="typeList"></param>
        /// <param name="configuration"></param>
        /// <param name="typeIgnore"></param>
        /// <param name="typeRegis"></param>
        public static void Register(this ContainerBuilder containerBuilder, IEnumerable<Type> typeList, IConfiguration configuration, List<string> typeIgnore, List<string> typeRegis)
        {
            foreach (var classType in typeList.Where(o => o.IsClass && !o.IsAbstract && o.IsImplementedType<ISettings>()).ToList())
            {
                try
                {
                    var attrList = classType.GetCustomAttributes<DIModeForSettingsAttribute>();
                    if (attrList == null || attrList.Count() == 0)
                        continue;

                    foreach (var attr in attrList)
                    {
                        var obj = configuration.GetSetting(attr.ConfigRootName, classType);
                        if (obj == null)
                            continue;

                        DIModeForServiceAttribute attr_tmp = null;
                        if (attr.KNFrom == Enum_DIKeyedNamedFrom.None)
                        {
                            attr_tmp = new DIModeForServiceAttribute(attr.EDIType, attr.Type, attr.Key);
                        }
                        else
                        {
                            var type = obj.GetType();
                            var pi = type.GetProperty(attr.Key.ToString());
                            var key = pi.GetValue(obj);

                            if (key == null) continue;

                            attr_tmp = new DIModeForServiceAttribute(attr.EDIType, attr.Type, key);
                        }

                        containerBuilder.Register(attr_tmp, obj, typeIgnore, typeRegis);
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 注入配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="containerBuilder"></param>
        /// <param name="objList"></param>
        /// <param name="typeIgnore"></param>
        /// <param name="typeRegis"></param>
        public static void Register<T>(this ContainerBuilder containerBuilder, IEnumerable<T> objList, List<string> typeIgnore, List<string> typeRegis)
        {
            foreach (var obj in objList)
            {
                try
                {
                    var classType = obj.GetType();

                    var attrList = obj.GetType().GetCustomAttributes<DIModeForSettingsAttribute>();
                    if (attrList == null || attrList.Count() == 0)
                        continue;

                    foreach (var attr in attrList)
                    {
                        DIModeForServiceAttribute attr_tmp = null;
                        if (attr.KNFrom == Enum_DIKeyedNamedFrom.None)
                        {
                            attr_tmp = new DIModeForServiceAttribute(attr.EDIType, attr.Type, attr.Key);
                        }
                        else
                        {
                            var pi = classType.GetProperty(attr.Key.ToString());
                            var key = pi.GetValue(obj);

                            attr_tmp = new DIModeForServiceAttribute(attr.EDIType, attr.Type, key);
                        }

                        containerBuilder.Register(attr_tmp, obj, typeIgnore, typeRegis);
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="typeList"></param>
        /// <param name="typeIgnore"></param>
        /// <param name="typeRegis"></param>
        public static void Register(this ContainerBuilder containerBuilder, IEnumerable<Type> typeList, List<string> typeIgnore, List<string> typeRegis)
        {
            foreach (var classType in typeList.Where(o => o.IsClass && !o.IsAbstract).ToList())
            {
                try
                {
                    var attrList = classType.GetCustomAttributes<DIModeForServiceAttribute>();
                    if (attrList == null || attrList.Count() == 0)
                    {
                        //var attr = new DIModeForServiceAttribute(Enum_DIType.AsImpl);
                        //containerBuilder.Register(attr, classType, typeIgnore, typeRegis);
                    }
                    else
                    {
                        foreach (var attr in attrList)
                            containerBuilder.Register(attr, classType, typeIgnore, typeRegis);
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 注入对象
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="diMode"></param>
        /// <param name="target"></param>
        /// <param name="typeIgnore"></param>
        /// <param name="typeRegis"></param>
        private static void Register(this ContainerBuilder containerBuilder, DIModeForServiceAttribute diMode, object target, List<string> typeIgnore, List<string> typeRegis)
        {
            Type classType = null;
            object classObj = null;

            if (target is Type)
                classType = target as Type;
            else
            {
                classType = target.GetType();
                classObj = target;
            }

            if (typeIgnore.Contains(classType.FullName))
                return;

            var classInterfaceArray = classType.GetInterfaces();

            if (classType.IsImplementedType(classInterfaceArray, typeof(IIOCIgnore)))
            {
                typeIgnore.Add(classType.FullName);
                return;
            }

            switch (diMode.EDIType)
            {
                case Enum_DIType.AsSelf:
                    #region AsSelf

                    if (classType.IsImplementedType(classInterfaceArray, typeof(ITransient)))
                    {
                        if (classObj != null)
                        {
                            containerBuilder.Register(o => classObj).AsSelf().InstancePerDependency();
                        }
                        else
                        {
                            if (!classType.IsGenericType)
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterType(classType).AsSelf().InstancePerDependency();
                                else
                                    containerBuilder.RegisterType(classType).AsSelf().WithParameters(diMode.Params).InstancePerDependency();
                            }
                            else
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterGeneric(classType).AsSelf().InstancePerDependency();
                                else
                                    containerBuilder.RegisterGeneric(classType).AsSelf().WithParameters(diMode.Params).InstancePerDependency();
                            }
                        }

                        typeRegis.Add(classType.FullName + "_AsSelf_");
                    }
                    else if (classType.IsImplementedType(classInterfaceArray, typeof(IScoped)))
                    {
                        if (classObj != null)
                        {
                            containerBuilder.Register(o => classObj).AsSelf().InstancePerLifetimeScope();
                        }
                        else
                        {
                            if (!classType.IsGenericType)
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterType(classType).AsSelf().InstancePerLifetimeScope();
                                else
                                    containerBuilder.RegisterType(classType).AsSelf().WithParameters(diMode.Params).InstancePerLifetimeScope();
                            }
                            else
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterGeneric(classType).AsSelf().InstancePerLifetimeScope();
                                else
                                    containerBuilder.RegisterGeneric(classType).AsSelf().WithParameters(diMode.Params).InstancePerLifetimeScope();
                            }
                        }

                        typeRegis.Add(classType.FullName + "_AsSelf_");
                    }
                    else if (classType.IsImplementedType(classInterfaceArray, typeof(ISingleton)))
                    {
                        if (classObj != null)
                        {
                            containerBuilder.Register(o => classObj).AsSelf().SingleInstance();
                        }
                        else
                        {
                            if (!classType.IsGenericType)
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterType(classType).AsSelf().SingleInstance();
                                else
                                    containerBuilder.RegisterType(classType).AsSelf().WithParameters(diMode.Params).SingleInstance();
                            }
                            else
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterGeneric(classType).AsSelf().SingleInstance();
                                else
                                    containerBuilder.RegisterGeneric(classType).AsSelf().WithParameters(diMode.Params).SingleInstance();
                            }
                        }

                        typeRegis.Add(classType.FullName + "_AsSelf_");
                    }
                    else
                    {
                        typeIgnore.Add(classType.FullName + "_AsSelf_");
                    }

                    #endregion
                    break;
                case Enum_DIType.AsImpl:
                    #region AsImpl

                    if (classType.IsImplementedType(classInterfaceArray, typeof(ITransient)))
                    {
                        if (classObj != null)
                        {
                            containerBuilder.Register(o => classObj).AsImplementedInterfaces().InstancePerDependency();
                        }
                        else
                        {
                            if (!classType.IsGenericType)
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterType(classType).AsImplementedInterfaces().InstancePerDependency();
                                else
                                    containerBuilder.RegisterType(classType).AsImplementedInterfaces().WithParameters(diMode.Params).InstancePerDependency();
                            }
                            else
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterGeneric(classType).AsImplementedInterfaces().InstancePerDependency();
                                else
                                    containerBuilder.RegisterGeneric(classType).AsImplementedInterfaces().WithParameters(diMode.Params).InstancePerDependency();
                            }
                        }

                        typeRegis.Add(classType.FullName + "_AsImpl_");
                    }
                    else if (classType.IsImplementedType(classInterfaceArray, typeof(IScoped)))
                    {
                        if (classObj != null)
                        {
                            containerBuilder.Register(o => classObj).AsImplementedInterfaces().InstancePerLifetimeScope();
                        }
                        else
                        {
                            if (!classType.IsGenericType)
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterType(classType).AsImplementedInterfaces().InstancePerLifetimeScope();
                                else
                                    containerBuilder.RegisterType(classType).AsImplementedInterfaces().WithParameters(diMode.Params).InstancePerLifetimeScope();
                            }
                            else
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterGeneric(classType).AsImplementedInterfaces().InstancePerLifetimeScope();
                                else
                                    containerBuilder.RegisterGeneric(classType).AsImplementedInterfaces().WithParameters(diMode.Params).InstancePerLifetimeScope();
                            }
                        }

                        typeRegis.Add(classType.FullName + "_AsImpl_");
                    }
                    else if (classType.IsImplementedType(classInterfaceArray, typeof(ISingleton)))
                    {
                        if (classObj != null)
                        {
                            containerBuilder.Register(o => classObj).AsImplementedInterfaces().SingleInstance();
                        }
                        else
                        {
                            if (!classType.IsGenericType)
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterType(classType).AsImplementedInterfaces().SingleInstance();
                                else
                                    containerBuilder.RegisterType(classType).AsImplementedInterfaces().WithParameters(diMode.Params).SingleInstance();
                            }
                            else
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterGeneric(classType).AsImplementedInterfaces().SingleInstance();
                                else
                                    containerBuilder.RegisterGeneric(classType).AsImplementedInterfaces().WithParameters(diMode.Params).SingleInstance();
                            }
                        }

                        typeRegis.Add(classType.FullName + "_AsImpl_");
                    }
                    else
                    {
                        typeIgnore.Add(classType.FullName + "_AsImpl_");
                    }

                    #endregion
                    break;
                case Enum_DIType.Exclusive:
                    #region Specific

                    if (classType.IsImplementedType(classInterfaceArray, typeof(ITransient)))
                    {
                        if (classObj != null)
                        {
                            containerBuilder.Register(o => classObj).As(diMode.Type).InstancePerDependency();
                        }
                        else
                        {
                            if (!classType.IsGenericType)
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterType(classType).As(diMode.Type).InstancePerDependency();
                                else
                                    containerBuilder.RegisterType(classType).As(diMode.Type).WithParameters(diMode.Params).InstancePerDependency();
                            }
                            else
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterGeneric(classType).As(diMode.Type).InstancePerDependency();
                                else
                                    containerBuilder.RegisterGeneric(classType).As(diMode.Type).WithParameters(diMode.Params).InstancePerDependency();
                            }
                        }

                        typeRegis.Add(classType.FullName + "_Exclusive_" + diMode.Type.FullName);
                    }
                    else if (classType.IsImplementedType(classInterfaceArray, typeof(IScoped)))
                    {
                        if (classObj != null)
                        {
                            containerBuilder.Register(o => classObj).As(diMode.Type).InstancePerLifetimeScope();
                        }
                        else
                        {
                            if (!classType.IsGenericType)
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterType(classType).As(diMode.Type).InstancePerLifetimeScope();
                                else
                                    containerBuilder.RegisterType(classType).As(diMode.Type).WithParameters(diMode.Params).InstancePerLifetimeScope();
                            }
                            else
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterGeneric(classType).As(diMode.Type).InstancePerLifetimeScope();
                                else
                                    containerBuilder.RegisterGeneric(classType).As(diMode.Type).WithParameters(diMode.Params).InstancePerLifetimeScope();
                            }
                        }

                        typeRegis.Add(classType.FullName + "_Exclusive_" + diMode.Type.FullName);
                    }
                    else if (classType.IsImplementedType(classInterfaceArray, typeof(ISingleton)))
                    {
                        if (classObj != null)
                        {
                            containerBuilder.Register(o => classObj).As(diMode.Type).SingleInstance();
                        }
                        else
                        {
                            if (!classType.IsGenericType)
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterType(classType).As(diMode.Type).SingleInstance();
                                else
                                    containerBuilder.RegisterType(classType).As(diMode.Type).WithParameters(diMode.Params).SingleInstance();
                            }
                            else
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterGeneric(classType).As(diMode.Type).SingleInstance();
                                else
                                    containerBuilder.RegisterGeneric(classType).As(diMode.Type).WithParameters(diMode.Params).SingleInstance();
                            }
                        }

                        typeRegis.Add(classType.FullName + "_Exclusive_" + diMode.Type.FullName);
                    }
                    else
                    {
                        typeIgnore.Add(classType.FullName + "_Exclusive_" + diMode.Type.FullName);
                    }

                    #endregion
                    break;
                case Enum_DIType.ExclusiveByNamed:
                    #region ExclusiveByNamed

                    if (classType.IsImplementedType(classInterfaceArray, typeof(ITransient)))
                    {
                        if (classObj != null)
                        {
                            containerBuilder.Register(o => classObj).Named(diMode.Key.ToString(), diMode.Type).InstancePerDependency();
                        }
                        else
                        {
                            if (!classType.IsGenericType)
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterType(classType).Named(diMode.Key.ToString(), diMode.Type).InstancePerDependency();
                                else
                                    containerBuilder.RegisterType(classType).Named(diMode.Key.ToString(), diMode.Type).WithParameters(diMode.Params).InstancePerDependency();
                            }
                            else
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterGeneric(classType).Named(diMode.Key.ToString(), diMode.Type).InstancePerDependency();
                                else
                                    containerBuilder.RegisterGeneric(classType).Named(diMode.Key.ToString(), diMode.Type).WithParameters(diMode.Params).InstancePerDependency();
                            }
                        }

                        typeRegis.Add(classType.FullName + "_ExclusiveByNamed_" + diMode.Type.FullName + "_" + diMode.Key.ToString());
                    }
                    else if (classType.IsImplementedType(classInterfaceArray, typeof(IScoped)))
                    {
                        if (classObj != null)
                        {
                            containerBuilder.Register(o => classObj).Named(diMode.Key.ToString(), diMode.Type).InstancePerLifetimeScope();
                        }
                        else
                        {
                            if (!classType.IsGenericType)
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterType(classType).Named(diMode.Key.ToString(), diMode.Type).InstancePerLifetimeScope();
                                else
                                    containerBuilder.RegisterType(classType).Named(diMode.Key.ToString(), diMode.Type).WithParameters(diMode.Params).InstancePerLifetimeScope();
                            }
                            else
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterGeneric(classType).Named(diMode.Key.ToString(), diMode.Type).InstancePerLifetimeScope();
                                else
                                    containerBuilder.RegisterGeneric(classType).Named(diMode.Key.ToString(), diMode.Type).WithParameters(diMode.Params).InstancePerLifetimeScope();
                            }
                        }

                        typeRegis.Add(classType.FullName + "_ExclusiveByNamed_" + diMode.Type.FullName + "_" + diMode.Key.ToString());
                    }
                    else if (classType.IsImplementedType(classInterfaceArray, typeof(ISingleton)))
                    {
                        if (classObj != null)
                        {
                            containerBuilder.Register(o => classObj).Named(diMode.Key.ToString(), diMode.Type).SingleInstance();
                        }
                        else
                        {
                            if (!classType.IsGenericType)
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterType(classType).Named(diMode.Key.ToString(), diMode.Type).SingleInstance();
                                else
                                    containerBuilder.RegisterType(classType).Named(diMode.Key.ToString(), diMode.Type).WithParameters(diMode.Params).SingleInstance();
                            }
                            else
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterGeneric(classType).Named(diMode.Key.ToString(), diMode.Type).SingleInstance();
                                else
                                    containerBuilder.RegisterGeneric(classType).Named(diMode.Key.ToString(), diMode.Type).WithParameters(diMode.Params).SingleInstance();
                            }
                        }

                        typeRegis.Add(classType.FullName + "_ExclusiveByNamed_" + diMode.Type.FullName + "_" + diMode.Key.ToString());
                    }
                    else
                    {
                        typeIgnore.Add(classType.FullName + "_ExclusiveByNamed_" + diMode.Type.FullName + "_" + diMode.Key.ToString());
                    }

                    #endregion
                    break;
                case Enum_DIType.ExclusiveByKeyed:
                    #region ExclusiveByKeyed

                    if (classType.IsImplementedType(classInterfaceArray, typeof(ITransient)))
                    {
                        if (classObj != null)
                        {
                            containerBuilder.Register(o => classObj).Keyed(diMode.Key, diMode.Type).InstancePerDependency();
                        }
                        else
                        {
                            if (!classType.IsGenericType)
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterType(classType).Keyed(diMode.Key, diMode.Type).InstancePerDependency();
                                else
                                    containerBuilder.RegisterType(classType).Keyed(diMode.Key, diMode.Type).WithParameters(diMode.Params).InstancePerDependency();
                            }
                            else
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterGeneric(classType).Keyed(diMode.Key, diMode.Type).InstancePerDependency();
                                else
                                    containerBuilder.RegisterGeneric(classType).Keyed(diMode.Key, diMode.Type).WithParameters(diMode.Params).InstancePerDependency();
                            }
                        }

                        typeRegis.Add(classType.FullName + "_ExclusiveByKeyed_" + diMode.Type.FullName + "_" + diMode.Key.ToString());
                    }
                    else if (classType.IsImplementedType(classInterfaceArray, typeof(IScoped)))
                    {
                        if (classObj != null)
                        {
                            containerBuilder.Register(o => classObj).Keyed(diMode.Key, diMode.Type).InstancePerLifetimeScope();
                        }
                        else
                        {
                            if (!classType.IsGenericType)
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterType(classType).Keyed(diMode.Key, diMode.Type).InstancePerLifetimeScope();
                                else
                                    containerBuilder.RegisterType(classType).Keyed(diMode.Key, diMode.Type).WithParameters(diMode.Params).InstancePerLifetimeScope();
                            }
                            else
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterGeneric(classType).Keyed(diMode.Key, diMode.Type).InstancePerLifetimeScope();
                                else
                                    containerBuilder.RegisterGeneric(classType).Keyed(diMode.Key, diMode.Type).WithParameters(diMode.Params).InstancePerLifetimeScope();
                            }
                        }

                        typeRegis.Add(classType.FullName + "_ExclusiveByKeyed_" + diMode.Type.FullName + "_" + diMode.Key.ToString());
                    }
                    else if (classType.IsImplementedType(classInterfaceArray, typeof(ISingleton)))
                    {
                        if (classObj != null)
                        {
                            containerBuilder.Register(o => classObj).Keyed(diMode.Key, diMode.Type).SingleInstance();
                        }
                        else
                        {
                            if (!classType.IsGenericType)
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterType(classType).Keyed(diMode.Key, diMode.Type).SingleInstance();
                                else
                                    containerBuilder.RegisterType(classType).Keyed(diMode.Key, diMode.Type).WithParameters(diMode.Params).SingleInstance();
                            }
                            else
                            {
                                if (diMode.Params == null || diMode.Params.Count() == 0)
                                    containerBuilder.RegisterGeneric(classType).Keyed(diMode.Key, diMode.Type).SingleInstance();
                                else
                                    containerBuilder.RegisterGeneric(classType).Keyed(diMode.Key, diMode.Type).WithParameters(diMode.Params).SingleInstance();
                            }
                        }

                        typeRegis.Add(classType.FullName + "_ExclusiveByKeyed_" + diMode.Type.FullName + "_" + diMode.Key.ToString());
                    }
                    else
                    {
                        typeIgnore.Add(classType.FullName + "_ExclusiveByKeyed_" + diMode.Type.FullName + "_" + diMode.Key.ToString());
                    }

                    #endregion
                    break;
                default:
                    typeIgnore.Add(classType.FullName);
                    break;
            }

            if (classObj != null)
            {
                var attr = classType.GetCustomAttribute<DIModeForArrayItemAttribute>();
                if (attr == null)
                    return;

                var objArray = classObj as IEnumerable<object>;
                Register(containerBuilder, objArray, typeIgnore, typeRegis);
            }
        }
    }
}
