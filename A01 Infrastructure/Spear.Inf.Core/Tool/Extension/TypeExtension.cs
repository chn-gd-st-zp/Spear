using System;
using System.Linq;

namespace Spear.Inf.Core.Tool
{
    public static class TypeExtension
    {
        /// <summary>
        /// 判断是否 实现 或 继承 某个泛型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
        public static bool IsGenericOf(this Type type, Type genericType)
        {
            // 匹配接口。
            var isTheRawGenericType = type.GetInterfaces().Any(o => genericType == (o.IsGenericType ? o.GetGenericTypeDefinition() : o));
            if (isTheRawGenericType)
                return true;

            // 匹配类型。
            while (type != typeof(object))
            {
                isTheRawGenericType = genericType == (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
                if (isTheRawGenericType)
                    return true;

                type = type.BaseType;
            }

            // 没有找到任何匹配的接口或类型。
            return false;
        }

        /// <summary>
        /// 判断是否 实现 某个类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="basicType"></param>
        /// <returns></returns>
        public static bool IsImplementedOf(this Type type, Type basicType)
        {
            bool result = false;

            result = basicType.IsAssignableFrom(type);
            if (result)
                return result;

            var types = type.GetInterfaces();
            foreach (var t in types)
            {
                if (!t.IsGenericType && !basicType.IsGenericType && t.FullName == basicType.FullName)
                    return true;

                if (t.IsGenericType && basicType.IsGenericType && t.Name == basicType.Name)
                {
                    if (t.GetGenericTypeDefinition() == basicType.GetGenericTypeDefinition())
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断是否 实现 某个类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static bool IsImplementedOf<T>(this Type objType)
        {
            return objType.IsImplementedOf(typeof(T));
        }

        /// <summary>
        /// 判断是否 继承 某个类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="basicType"></param>
        /// <returns></returns>
        public static bool IsExtendOf(this Type type, Type basicType)
        {
            return type == basicType || type.IsSubclassOf(basicType);
        }

        /// <summary>
        /// 判断是否 实现 某个类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static bool IsExtendOf<T>(this Type objType)
        {
            return objType.IsExtendOf(typeof(T));
        }
    }
}
