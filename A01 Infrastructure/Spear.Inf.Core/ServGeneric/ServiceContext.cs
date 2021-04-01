using System;
using System.Collections.Generic;

using Autofac;
using Autofac.Core;

using Spear.Inf.Core.ServGeneric.MicServ;

namespace Spear.Inf.Core.ServGeneric
{
    public class ServiceContext
    {
        private static IServiceProvider ServiceProvider;

        public static bool IsDoneLoad()
        {
            return ServiceProvider == null;
        }

        #region 服务 - 程序

        public static void InitServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }


        public static object Resolve(Type type)
        {
            try
            {
                if (ServiceProvider == null)
                    return default;

                var obj = ServiceProvider.GetService(type.GetType());
                if (obj == null)
                    return default;

                return obj;
            }
            catch
            {
                return default;
            }
        }

        public static T Resolve<T>()
        {
            try
            {
                if (ServiceProvider == null)
                    return default;

                var obj = ServiceProvider.GetService(typeof(T));
                if (obj == null)
                    return default;

                return (T)obj;
            }
            catch
            {
                return default;
            }
        }


        public static object ResolveServ(Type type)
        {
            try
            {
                var context = Resolve<IComponentContext>();
                if (context == null)
                    return default;

                return context.Resolve(type);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveServ<T>()
        {
            try
            {
                var context = Resolve<IComponentContext>();
                if (context == null)
                    return default;

                return context.Resolve<T>();
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveServ<T>(params Parameter[] parameters)
        {
            try
            {
                var context = Resolve<IComponentContext>();
                if (context == null)
                    return default;

                return context.Resolve<T>(parameters);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveServ<T>(IEnumerable<Parameter> parameters)
        {
            try
            {
                var context = Resolve<IComponentContext>();
                if (context == null)
                    return default;

                return context.Resolve<T>(parameters);
            }
            catch
            {
                return default;
            }
        }


        public static object ResolveServByNamed(Type type, string named)
        {
            try
            {
                var context = Resolve<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveNamed(named, type);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveServByNamed<T>(string named)
        {
            try
            {
                var context = Resolve<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveNamed<T>(named);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveServByNamed<T>(string named, params Parameter[] parameters)
        {
            try
            {
                var context = Resolve<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveNamed<T>(named, parameters);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveServByNamed<T>(string named, IEnumerable<Parameter> parameters)
        {
            try
            {
                var context = Resolve<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveNamed<T>(named, parameters);
            }
            catch
            {
                return default;
            }
        }


        public static object ResolveServByKeyed(Type type, string named)
        {
            try
            {
                var context = Resolve<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveKeyed(named, type);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveServByKeyed<T>(object keyed)
        {
            try
            {
                var context = Resolve<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveKeyed<T>(keyed);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public static T ResolveServByKeyed<T>(object keyed, params Parameter[] parameters)
        {
            try
            {
                var context = Resolve<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveKeyed<T>(keyed, parameters);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveServByKeyed<T>(object keyed, IEnumerable<Parameter> parameters)
        {
            try
            {
                var context = Resolve<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveKeyed<T>(keyed, parameters);
            }
            catch
            {
                return default;
            }
        }

        #endregion

        #region 服务 - 微服务

        public static void InitMicServClient()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        }

        public static T GetServ<T>(params object[] paramArray) where T : IMicServ<T>
        {
            var micServGener = ResolveServ<IMicServGener>();
            if (micServGener == null)
                return default;

            return micServGener.GetServ<T>(paramArray);
        }

        #endregion
    }
}
