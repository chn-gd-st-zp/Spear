using System;
using System.Collections.Generic;
using System.Linq;

using StackExchange.Redis;

using Spear.Inf.Core.Tool;

namespace Spear.MidM.Redis
{
    public class SERedis : ICache4Redis
    {
        private readonly RedisSettings _redisSettings;
        private readonly int _defaultDatabase;

        public SERedis(RedisSettings redisSettings, int defaultDatabase = -1)
        {
            _redisSettings = redisSettings;
            _defaultDatabase = defaultDatabase == -1 ? redisSettings.DefaultDatabase : defaultDatabase;
        }

        private ConnectionMultiplexer _connection
        {
            get
            {
                if (_connectionCache == null || !_connectionCache.IsConnected)
                {
                    _connectionCache = ConnectionMultiplexer.Connect(_redisSettings.Connection);
                    _connectionCache.PreserveAsyncOrder = false;
                }

                return _connectionCache;
            }
        }
        private ConnectionMultiplexer _connectionCache = null;

        /// <summary>
        /// 销毁连接
        /// </summary>
        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        private IDatabase GetDB(int? dbIndex = null)
        {
            var index = _defaultDatabase;

            if (dbIndex.HasValue)
                index = dbIndex.Value;

            return _connection.GetDatabase(_defaultDatabase);
        }

        /// <summary>
        /// 取得redis的Key名称
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetKeyForRedis(string key)
        {
            return _redisSettings.InstanceName + key;
        }

        /// <summary>
        /// 设置缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, TimeSpan ts)
        {
            return Set(_defaultDatabase, key, value, ts);
        }

        /// <summary>
        /// 设置缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool Set<T>(int dbIndex, string key, T value, TimeSpan ts)
        {
            if (key.IsEmptyString())
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var finalKey = GetKeyForRedis(key);

            return GetDB(dbIndex).StringSet(finalKey, value.ToJson(), ts);
        }

        /// <summary>
        /// 设置缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set<T>(string key, T value)
        {
            return Set(_defaultDatabase, key, value);
        }

        /// <summary>
        /// 设置缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set<T>(int dbIndex, string key, T value)
        {
            DateTime now = DateTime.Now;
            TimeSpan validDuration = now.AddMinutes(_redisSettings.DefaultTimeOutMinutes) - now;
            return Set(dbIndex, key, value, validDuration);
        }

        /// <summary>
        /// 设置相对过期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, int second)
        {
            return Set(_defaultDatabase, key, value, second);
        }

        /// <summary>
        /// 设置相对过期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public bool Set<T>(int dbIndex, string key, T value, int second)
        {
            DateTime now = DateTime.Now;
            TimeSpan validDuration = now.AddSeconds(second) - now;
            return Set(dbIndex, key, value, validDuration);
        }

        /// <summary>
        /// 设置绝对过期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, DateTime endTime)
        {
            return Set(_defaultDatabase, key, value, endTime);
        }

        /// <summary>
        /// 设置绝对过期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public bool Set<T>(int dbIndex, string key, T value, DateTime endTime)
        {
            TimeSpan validDuration = endTime - DateTimeOffset.Now;
            return Set(dbIndex, key, value, validDuration);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Del(string key)
        {
            return Del(_defaultDatabase, key);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Del(int dbIndex, string key)
        {
            return Set(dbIndex, key, "", DateTime.Now.AddSeconds(1));
        }

        /// <summary>
        /// 取得缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return Get<T>(_defaultDatabase, key);
        }

        /// <summary>
        /// 取得缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(int dbIndex, string key)
        {
            if (key.IsEmptyString())
                throw new ArgumentNullException(nameof(key));

            var finalKey = GetKeyForRedis(key);

            var value = GetDB(dbIndex).StringGet(finalKey);
            if (!value.HasValue)
                return default(T);

            return value.ToString().ToObject<T>();
        }

        /// <summary>
        /// 获取string类型的缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return Get(_defaultDatabase, key);
        }

        /// <summary>
        /// 获取string类型的缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public string Get(int dbIndex, string key)
        {
            if (key.IsEmptyString())
                throw new ArgumentNullException(nameof(key));

            var finalKey = GetKeyForRedis(key);

            var value = GetDB(dbIndex).StringGet(finalKey);
            if (!value.HasValue)
                return "";

            return value.ToString();
        }

        /// <summary>
        /// 判断当前Key是否存在数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return Exists(_defaultDatabase, key);
        }

        /// <summary>
        /// 判断当前Key是否存在数据
        /// </summary>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(int dbIndex, string key)
        {
            if (key.IsEmptyString())
                throw new ArgumentNullException(nameof(key));
            return GetDB(dbIndex).KeyExists(GetKeyForRedis(key));
        }

        /// <summary>
        /// 获取标识
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public List<string> Keys(string pattern = "*")
        {
            return Keys(_defaultDatabase, pattern);
        }

        /// <summary>
        /// 获取标识
        /// </summary>
        /// <param name="dbIndex"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public List<string> Keys(int dbIndex, string pattern = "*")
        {
            List<string> result = new List<string>();

            var redisResult = GetDB(dbIndex).ScriptEvaluate(LuaScript.Prepare("return redis.call('KEYS', @keypattern)"), new { @keypattern = pattern });

            if (!redisResult.IsNull)
            {
                RedisKey[] keys = (RedisKey[])redisResult;

                result = keys.Select(o => o.ToString()).ToList();
            }

            return result;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public List<T> List<T>(string pattern = "*")
        {
            return List<T>(_defaultDatabase, pattern);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public List<T> List<T>(int dbIndex, string pattern = "*")
        {
            List<T> result = new List<T>();

            var redisResult = GetDB(dbIndex).ScriptEvaluate(LuaScript.Prepare("return redis.call('KEYS', @keypattern)"), new { @keypattern = pattern });

            if (!redisResult.IsNull)
            {
                RedisKey[] keys = (RedisKey[])redisResult;

                var data_tmp1 = GetDB(dbIndex).StringGet(keys);

                List<string> data_tmp2 = data_tmp1.Select(o => o.ToString()).ToList();

                result = data_tmp2.Select(o => o.IsEmptyString() ? default(T) : o.ToObject<T>()).ToList();
            }

            return result;
        }

        /// <summary>
        /// 向队列末端增加对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public long ListRightPush<T>(string key, T obj)
        {
            return ListRightPush(_defaultDatabase, key, obj);
        }

        /// <summary>
        /// 向队列末端增加对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public long ListRightPush<T>(int dbIndex, string key, T obj)
        {
            var finalKey = GetKeyForRedis(key);
            return GetDB(dbIndex).ListRightPush(finalKey, obj.ToJson());
        }

        /// <summary>
        /// 从队列中移除对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public long ListRemove<T>(string key, T obj)
        {
            return ListRemove(_defaultDatabase, key, obj);
        }

        /// <summary>
        /// 从队列中移除对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public long ListRemove<T>(int dbIndex, string key, T obj)
        {
            var finalKey = GetKeyForRedis(key);
            return GetDB(dbIndex).ListRemove(finalKey, obj.ToJson());
        }

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> ListRange<T>(string key)
        {
            return ListRange<T>(_defaultDatabase, key);
        }

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> ListRange<T>(int dbIndex, string key)
        {
            List<T> result = new List<T>();

            key = GetKeyForRedis(key);
            var values = GetDB(dbIndex).ListRange(key, 0).Select(o => o.ToString()).ToArray();

            foreach (var value in values)
            {
                var obj = value.IsEmptyString() ? default : value.ToObject<T>();
                result.Add(obj);
            }

            return result;
        }
    }
}
