﻿using System;
using System.Linq;

using Autofac;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.CusException;
using Spear.Inf.Core.EncryptionNDecrypt;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.Tool;
using Spear.MidM.Redis;

namespace Spear.MidM.SessionNAuth
{
    public class SessionNAuth<T> : ISessionNAuth<T> where T : ITokenProvider
    {
        private readonly SessionNAuthSettings _sessionNAuthSettings;
        private readonly ICache4Redis _cache;

        public SessionNAuth()
        {
            var redisSettings = ServiceContext.Resolve<RedisSettings>();

            _sessionNAuthSettings = ServiceContext.Resolve<SessionNAuthSettings>();
            //_cache = ServiceContext.Resolve<ICache4Redis>(new NamedPropertyParameter("redisSettings", redisSettings), new NamedPropertyParameter("defaultDatabase", _sessionNAuthSettings.CacheDBIndex));
            _cache = ServiceContext.Resolve<ICache4Redis>(new TypedParameter(typeof(RedisSettings), redisSettings), new TypedParameter(typeof(int), _sessionNAuthSettings.CacheDBIndex));
        }

        public string CurToken
        {
            get
            {
                try
                {
                    if (_curToken.IsEmptyString())
                    {
                        var token1 = ServiceContext.Resolve<T>().CurToken;
                        var token2 = token1.IsEmptyString() ? "" : token1.ToString();
                        var token = token2 == "null" ? "" : token2;
                        _curToken = token;
                    }

                    return _curToken;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private string _curToken;

        public UserTokenRunTime CurUserToken
        {
            get
            {
                try
                {
                    if (_curUserToken == null)
                    {
                        string token = CurToken;
                        if (token.IsEmptyString())
                            throw new Exception_EmptyToken();

                        UserTokenCache userTokenCache = GetUserToken(token);
                        if (userTokenCache == null)
                            throw new Exception_NoLogin();

                        _curUserToken = userTokenCache.MapTo<UserTokenCache, UserTokenRunTime>();

                        _curUserToken.UpdateTime = DateTime.Now;
                    }

                    return _curUserToken;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    SetUserToken(_curUserToken.MapTo<UserTokenRunTime, UserTokenCache>());
                }
            }
        }
        private UserTokenRunTime _curUserToken;

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="accessToken">Token</param>
        /// <returns></returns>
        public UserTokenCache GetUserToken(string accessToken)
        {
            if (accessToken.IsEmptyString())
                return null;

            accessToken = MD5.Encrypt(accessToken);

            return _cache.Get<UserTokenCache>(_sessionNAuthSettings.CachePrefix + accessToken);
        }

        /// <summary>
        /// 设置Token
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public void SetUserToken(UserTokenCache userToken)
        {
            if (userToken == null)
                return;

            var time = TimeSpan.FromMinutes(_sessionNAuthSettings.CacheValidDuration);

            var accessToken = MD5.Encrypt(userToken.AccessToken);

            _cache.Set(_sessionNAuthSettings.CachePrefix + accessToken, userToken, time);
            _curToken = userToken.AccessToken;
        }

        /// <summary>
        /// 移除Token
        /// </summary>
        /// <param name="accessToken">Token</param>
        /// <returns></returns>
        public void RemoveUserToken(string accessToken)
        {
            if (accessToken.IsEmptyString())
                return;

            accessToken = MD5.Encrypt(accessToken, 32);

            _cache.Del(_sessionNAuthSettings.CachePrefix + accessToken);
        }

        /// <summary>
        /// 权限认证
        /// </summary>
        public void PermissionAuth(string permissionCode)
        {
            try
            {
                if (CurUserToken.ERoleType == Enum_Role.SuperAdmin)
                    return;

                if (CurUserToken.PermissionCodes.Contains(permissionCode))
                    return;

                throw new Exception_NoAuth();
            }
            catch (Exception ex)
            {
                throw new Exception("运行出错[权限认证]", ex);
            }
        }
    }
}
