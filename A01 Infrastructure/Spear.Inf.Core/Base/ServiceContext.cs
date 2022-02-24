using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

using Autofac;
using Autofac.Core;

namespace Spear.Inf.Core
{
    public class ServiceContext
    {
        public static bool IsDoneLoad { get { return _serviceProvider != null; } }

        private static IServiceProvider _serviceProvider;

        public static void InitServiceProvider(IServiceProvider serviceProvider) { _serviceProvider = serviceProvider; }

        public static IServiceScope CreateScope() { return _serviceProvider.CreateScope(); }


        public static object Resolve(Type type) { return _serviceProvider.Resolve(type); }

        public static T Resolve<T>() { return _serviceProvider.Resolve<T>(); }

        public static T Resolve<T>(params Parameter[] parameters) { return _serviceProvider.Resolve<T>(parameters); }

        public static T Resolve<T>(IEnumerable<Parameter> parameters) { return _serviceProvider.Resolve<T>(parameters); }


        public static object ResolveByNamed(Type type, string named) { return _serviceProvider.ResolveByNamed(type, named); }

        public static T ResolveByNamed<T>(string named) { return _serviceProvider.ResolveByNamed<T>(named); }

        public static T ResolveByNamed<T>(string named, params Parameter[] parameters) { return _serviceProvider.ResolveByNamed<T>(named, parameters); }

        public static T ResolveByNamed<T>(string named, IEnumerable<Parameter> parameters) { return _serviceProvider.ResolveByNamed<T>(named, parameters); }


        public static object ResolveByKeyed(Type type, object keyed) { return _serviceProvider.ResolveByKeyed(type, keyed); }

        public static T ResolveByKeyed<T>(object keyed) { return _serviceProvider.ResolveByKeyed<T>(keyed); }

        public static T ResolveByKeyed<T>(object keyed, params Parameter[] parameters) { return _serviceProvider.ResolveByKeyed<T>(keyed, parameters); }

        public static T ResolveByKeyed<T>(object keyed, IEnumerable<Parameter> parameters) { return _serviceProvider.ResolveByKeyed<T>(keyed, parameters); }
    }

    public static class IServiceScopeExtend
    {
        public static object Resolve(this IServiceScope serviceScope, Type type) { return serviceScope.ServiceProvider.Resolve(type); }

        public static T Resolve<T>(this IServiceScope serviceScope) { return serviceScope.ServiceProvider.Resolve<T>(); }

        public static T Resolve<T>(this IServiceScope serviceScope, params Parameter[] parameters) { return serviceScope.ServiceProvider.Resolve<T>(parameters); }

        public static T Resolve<T>(this IServiceScope serviceScope, IEnumerable<Parameter> parameters) { return serviceScope.ServiceProvider.Resolve<T>(parameters); }


        public static object ResolveByNamed(this IServiceScope serviceScope, Type type, string named) { return serviceScope.ServiceProvider.ResolveByNamed(type, named); }

        public static T ResolveByNamed<T>(this IServiceScope serviceScope, string named) { return serviceScope.ServiceProvider.ResolveByNamed<T>(named); }

        public static T ResolveByNamed<T>(this IServiceScope serviceScope, string named, params Parameter[] parameters) { return serviceScope.ServiceProvider.ResolveByNamed<T>(named, parameters); }

        public static T ResolveByNamed<T>(this IServiceScope serviceScope, string named, IEnumerable<Parameter> parameters) { return serviceScope.ServiceProvider.ResolveByNamed<T>(named, parameters); }


        public static object ResolveByKeyed(this IServiceScope serviceScope, Type type, object keyed) { return serviceScope.ServiceProvider.ResolveByKeyed(type, keyed); }

        public static T ResolveByKeyed<T>(this IServiceScope serviceScope, object keyed) { return serviceScope.ServiceProvider.ResolveByKeyed<T>(keyed); }

        public static T ResolveByKeyed<T>(this IServiceScope serviceScope, object keyed, params Parameter[] parameters) { return serviceScope.ServiceProvider.ResolveByKeyed<T>(keyed, parameters); }

        public static T ResolveByKeyed<T>(this IServiceScope serviceScope, object keyed, IEnumerable<Parameter> parameters) { return serviceScope.ServiceProvider.ResolveByKeyed<T>(keyed, parameters); }
    }

    public static class IServiceProviderExtend
    {
        public static object GetService(this IServiceProvider serviceProvider, Type type)
        {
            try
            {
                if (serviceProvider == null)
                    return default;

                var obj = serviceProvider.GetService(type.GetType());
                if (obj == null)
                    return default;

                return obj;
            }
            catch
            {
                return default;
            }
        }

        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            try
            {
                if (serviceProvider == null)
                    return default;

                var obj = serviceProvider.GetService(typeof(T));
                if (obj == null)
                    return default;

                return (T)obj;
            }
            catch
            {
                return default;
            }
        }


        public static object Resolve(this IServiceProvider serviceProvider, Type type)
        {
            try
            {
                var context = serviceProvider.GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.Resolve(type);
            }
            catch
            {
                return default;
            }
        }

        public static T Resolve<T>(this IServiceProvider serviceProvider)
        {
            try
            {
                var context = serviceProvider.GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.Resolve<T>();
            }
            catch
            {
                return default;
            }
        }

        public static T Resolve<T>(this IServiceProvider serviceProvider, params Parameter[] parameters)
        {
            try
            {
                var context = serviceProvider.GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.Resolve<T>(parameters);
            }
            catch
            {
                return default;
            }
        }

        public static T Resolve<T>(this IServiceProvider serviceProvider, IEnumerable<Parameter> parameters)
        {
            try
            {
                var context = serviceProvider.GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.Resolve<T>(parameters);
            }
            catch
            {
                return default;
            }
        }


        public static object ResolveByNamed(this IServiceProvider serviceProvider, Type type, string named)
        {
            try
            {
                var context = serviceProvider.GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveNamed(named, type);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveByNamed<T>(this IServiceProvider serviceProvider, string named)
        {
            try
            {
                var context = serviceProvider.GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveNamed<T>(named);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveByNamed<T>(this IServiceProvider serviceProvider, string named, params Parameter[] parameters)
        {
            try
            {
                var context = serviceProvider.GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveNamed<T>(named, parameters);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveByNamed<T>(this IServiceProvider serviceProvider, string named, IEnumerable<Parameter> parameters)
        {
            try
            {
                var context = serviceProvider.GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveNamed<T>(named, parameters);
            }
            catch
            {
                return default;
            }
        }


        public static object ResolveByKeyed(this IServiceProvider serviceProvider, Type type, object keyed)
        {
            try
            {
                var context = serviceProvider.GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveKeyed(keyed, type);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveByKeyed<T>(this IServiceProvider serviceProvider, object keyed)
        {
            try
            {
                var context = serviceProvider.GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveKeyed<T>(keyed);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveByKeyed<T>(this IServiceProvider serviceProvider, object keyed, params Parameter[] parameters)
        {
            try
            {
                var context = serviceProvider.GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveKeyed<T>(keyed, parameters);
            }
            catch
            {
                return default;
            }
        }

        public static T ResolveByKeyed<T>(this IServiceProvider serviceProvider, object keyed, IEnumerable<Parameter> parameters)
        {
            try
            {
                var context = serviceProvider.GetService<IComponentContext>();
                if (context == null)
                    return default;

                return context.ResolveKeyed<T>(keyed, parameters);
            }
            catch
            {
                return default;
            }
        }
    }
}
