using System;
using System.Collections.Generic;
using System.Linq;

using StackExchange.Redis;

using Spear.Inf.Core.Tool;

using CUS = Spear.Inf.Core.Interface;

namespace Spear.MidM.Redis
{
    public class SERedis : CUS.ICache, CUS.ICache4Redis
    {
        private readonly RedisSettings _redisSettings;
        private ConnectionMultiplexer _connection = null;

        public SERedis(RedisSettings redisSettings)
        {
            _redisSettings = redisSettings;
        }

        /// <summary>
        /// 数据库
        /// </summary>
        private IDatabase DB
        {
            get
            {
                if (_connection == null || !_connection.IsConnected)
                {
                    _connection = ConnectionMultiplexer.Connect(_redisSettings.Connection);
                    _connection.PreserveAsyncOrder = false;
                }

                return _connection.GetDatabase(_redisSettings.DefaultDatabase);
            }
        }

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
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set<T>(string key, T value)
        {
            DateTime now = DateTime.Now;
            TimeSpan validDuration = now.AddMinutes(_redisSettings.DefaultTimeOutMinutes) - now;
            return Set(key, value, validDuration);
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
            DateTime now = DateTime.Now;
            TimeSpan validDuration = now.AddSeconds(second) - now;
            return Set(key, value, validDuration);
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
            TimeSpan validDuration = endTime - DateTimeOffset.Now;
            return Set(key, value, validDuration);
        }

        /// <summary>
        /// 设置缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, TimeSpan ts)
        {
            if (key.IsEmptyString())
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var finalKey = GetKeyForRedis(key);

            return DB.StringSet(finalKey, value.ToJson(), ts);
        }

        /// <summary>
        /// 移除redis
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Del(string key)
        {
            return Set(key, "", DateTime.Now.AddSeconds(1));
        }

        /// <summary>
        /// 取得缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (key.IsEmptyString())
                throw new ArgumentNullException(nameof(key));

            var finalKey = GetKeyForRedis(key);

            var value = DB.StringGet(finalKey);
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
            if (key.IsEmptyString())
                throw new ArgumentNullException(nameof(key));

            var finalKey = GetKeyForRedis(key);

            var value = DB.StringGet(finalKey);
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
            if (key.IsEmptyString())
                throw new ArgumentNullException(nameof(key));
            return DB.KeyExists(GetKeyForRedis(key));
        }

        /// <summary>
        /// 获取标识
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public List<string> Keys(string pattern = "*")
        {
            List<string> result = new List<string>();

            var redisResult = DB.ScriptEvaluate(LuaScript.Prepare("return redis.call('KEYS', @keypattern)"), new { @keypattern = pattern });

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
            List<T> result = new List<T>();

            var redisResult = DB.ScriptEvaluate(LuaScript.Prepare("return redis.call('KEYS', @keypattern)"), new { @keypattern = pattern });

            if (!redisResult.IsNull)
            {
                RedisKey[] keys = (RedisKey[])redisResult;

                var data_tmp1 = DB.StringGet(keys);

                List<string> data_tmp2 = data_tmp1.Select(o => o.ToString()).ToList();

                result = data_tmp2.Select(o => o.IsEmptyString() ? default(T) : o.ToObject<T>()).ToList();
            }

            return result;
        }

        public long ListRightPush<T>(string key, T obj)
        {
            var finalKey = GetKeyForRedis(key);
            return DB.ListRightPush(finalKey, obj.ToJson());
        }

        public long ListRemove<T>(string key, T obj)
        {
            var finalKey = GetKeyForRedis(key);
            return DB.ListRemove(finalKey, obj.ToJson());
        }

        public List<T> ListRange<T>(string key)
        {
            List<T> result = new List<T>();

            key = GetKeyForRedis(key);
            var values = DB.ListRange(key, 0).Select(o => o.ToString()).ToArray();

            foreach (var value in values)
            {
                var obj = value.IsEmptyString() ? default : value.ToObject<T>();
                result.Add(obj);
            }

            return result;
        }
    }
}
