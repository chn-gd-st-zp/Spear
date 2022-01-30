using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using AutoMapper;
using Mapster;

namespace Spear.Inf.Core.Tool
{
    public static class ModelConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public static object TypeTo(this object obj, Type destinationType)
        {
            if (!destinationType.IsGenericType)
            {
                return Convert.ChangeType(obj, destinationType);
            }
            else
            {
                Type genericTypeDefinition = destinationType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                    return Convert.ChangeType(obj, Nullable.GetUnderlyingType(destinationType));
            }

            throw new InvalidCastException(string.Format("Invalid cast from type \"{0}\" to type \"{1}\".", obj.GetType().FullName, destinationType.FullName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object TypeTo<TDestination>(this object obj)
        {
            return TypeTo(obj, typeof(TDestination));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="byProfile"></param>
        /// <returns></returns>
        public static TDestination MapTo<TDestination>(this object source, bool byProfile = false)
            where TDestination : class
        {
            if (source == null) return default(TDestination);

            Type tSource = source.GetType();
            Type tTarget = typeof(TDestination);

            IMapper mapper = byProfile ? ServiceContext.Resolve<IMapper>() : new MapperConfiguration(cfg => cfg.CreateMap(tSource, tTarget)).CreateMapper();

            return mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="byProfile"></param>
        /// <returns></returns>
        public static IEnumerable<TDestination> MapTo<TSource, TDestination>(this IEnumerable<TSource> source, bool byProfile = false)
            where TSource : class
            where TDestination : class
        {
            return source.Select(o => o.MapTo<TDestination>(byProfile));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination AdaptTo<TDestination>(this object source)
        {
            return source.AdaptTo<TDestination>(TypeAdapterConfig.GlobalSettings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static TDestination AdaptTo<TDestination>(this object source, TypeAdapterConfig config)
        {
            if (source == null) return default(TDestination);

            return config.GetDynamicMapFunction<TDestination>(source.GetType())(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination AdaptTo<TSource, TDestination>(this TSource source)
        {
            return TypeAdapter<TSource, TDestination>.Map(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static TDestination AdaptTo<TSource, TDestination>(this TSource source, TypeAdapterConfig config)
        {
            return config.GetMapFunction<TSource, TDestination>()(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static TDestination AdaptTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return source.AdaptTo(destination, TypeAdapterConfig.GlobalSettings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static TDestination AdaptTo<TSource, TDestination>(this TSource source, TDestination destination, TypeAdapterConfig config)
        {
            return config.GetMapToTargetFunction<TSource, TDestination>()(source, destination);
        }
    }
}
