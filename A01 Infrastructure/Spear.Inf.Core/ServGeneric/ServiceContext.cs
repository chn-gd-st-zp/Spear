using System;
using System.Collections.Generic;

using Autofac;
using Autofac.Core;

using Spear.Inf.Core.ServGeneric.MicServ;

namespace Spear.Inf.Core.ServGeneric
{
    public class ServiceContext
    {
        public static bool IsDoneLoad { get { return ServiceProvider == null; } }

        private static IServiceProvider ServiceProvider;

        private static object GetService(Type type)
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

        private static T GetService<T>()
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

        #region 服务 - 程序

        public static void InitServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public static object Resolve(Type type)
        {
            try
            {
                var context = GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.Resolve(type);
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
                var context = GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.Resolve<T>();
            }
            catch
            {
                return default;
            }
        }

        public static T Resolve<T>(params Parameter[] parameters)
        {
            try
            {
                var context = GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.Resolve<T>(parameters);
            }
            catch
            {
                return default;
            }
        }

        public static T Resolve<T>(IEnumerable<Parameter> parameters)
        {
            try
            {
                var context = GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.Resolve<T>(parameters);
            }
            catch
            {
                return default;
            }
        }


        public static object ResolveByNamed(Type type, string named)
        {
            try
            {
                var context = GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveNamed(named, type);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveByNamed<T>(string named)
        {
            try
            {
                var context = GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveNamed<T>(named);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveByNamed<T>(string named, params Parameter[] parameters)
        {
            try
            {
                var context = GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveNamed<T>(named, parameters);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveByNamed<T>(string named, IEnumerable<Parameter> parameters)
        {
            try
            {
                var context = GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveNamed<T>(named, parameters);
            }
            catch
            {
                return default;
            }
        }


        public static object ResolveByKeyed(Type type, string named)
        {
            try
            {
                var context = GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveKeyed(named, type);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveByKeyed<T>(object keyed)
        {
            try
            {
                var context = GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveKeyed<T>(keyed);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public static T ResolveByKeyed<T>(object keyed, params Parameter[] parameters)
        {
            try
            {
                var context = GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveKeyed<T>(keyed, parameters);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveByKeyed<T>(object keyed, IEnumerable<Parameter> parameters)
        {
            try
            {
                var context = GetService<IComponentContext>();
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

        public static T ResolveMicServ<T>(params object[] paramArray) where T : IMicServ<T>
        {
            var micServGener = Resolve<IMicServGeneric>();
            if (micServGener == null)
                return default;

            return micServGener.GetServ<T>(paramArray);
        }

        #endregion
    }
}
