using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Spear.Inf.Core.Tool
{
    /// <summary>
    /// 实例构造器
    /// </summary>
    public static class InstanceCreator
    {
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>()
        {
            return Activator.CreateInstance<T>();
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object Create(Type type, params object[] args)
        {
            return Activator.CreateInstance(type, args);
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object CreateGenericType(Type type, Type genericType, params object[] args)
        {
            type = type.MakeGenericType(genericType);
            return Activator.CreateInstance(type, args);
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static object Create(string assemblyName, string className)
        {
            try
            {
                Assembly assembly = Assembly.Load(assemblyName);
                if (assembly == null)
                    throw new ApplicationException("找不到应用程序：" + assemblyName);

                object obj = assembly.CreateInstance(className);
                if (obj == null)
                    throw new ApplicationException("找不到类：" + className);

                return obj;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 深复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T obj)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, obj);
                memoryStream.Position = 0;
                object result = formatter.Deserialize(memoryStream);
                return (T)result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 深复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(this object obj)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, obj);
                memoryStream.Position = 0;
                object result = formatter.Deserialize(memoryStream);
                return (T)result;
            }
            catch
            {
                throw;
            }
        }
    }
}
