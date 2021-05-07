using System;
using System.Collections.Generic;

using Spear.Inf.Core.Interface;

namespace Spear.MidM.Redis
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache4Redis : ICache
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        bool Set<T>(int dbIndex, string key, T value, TimeSpan ts);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Set<T>(int dbIndex, string key, T value);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        bool Set<T>(int dbIndex, string key, T value, int second);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        bool Set<T>(int dbIndex, string key, T value, DateTime endTime);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <returns></returns>
        bool Del(int dbIndex, string key);

        /// <summary>
        /// 取得缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(int dbIndex, string key);

        /// <summary>
        /// 获取string类型的缓存值
        /// </summary>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(int dbIndex, string key);

        /// <summary>
        /// 判断当前Key是否存在数据
        /// </summary>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(int dbIndex, string key);

        /// <summary>
        /// 获取标识
        /// </summary>
        /// <param name="dbIndex"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        List<string> Keys(int dbIndex, string pattern = "*");

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        List<T> List<T>(int dbIndex, string pattern = "*");

        /// <summary>
        /// 向队列末端增加对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        long ListRightPush<T>(string key, T obj);

        /// <summary>
        /// 向队列末端增加对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        long ListRightPush<T>(int dbIndex, string key, T obj);

        /// <summary>
        /// 从队列中移除对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        long ListRemove<T>(string key, T obj);

        /// <summary>
        /// 从队列中移除对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        long ListRemove<T>(int dbIndex, string key, T obj);

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        List<T> ListRange<T>(string key);

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        List<T> ListRange<T>(int dbIndex, string key);
    }
}
