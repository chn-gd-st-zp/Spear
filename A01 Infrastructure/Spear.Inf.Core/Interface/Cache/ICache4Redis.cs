using System.Collections.Generic;

namespace Spear.Inf.Core.Interface
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache4Redis : ICache
    {
        /// <summary>
        /// 向队列末端增加对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        long ListRightPush<T>(string key, T obj);

        /// <summary>
        /// 从队列中移除对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        long ListRemove<T>(string key, T obj);

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        List<T> ListRange<T>(string key);
    }
}
