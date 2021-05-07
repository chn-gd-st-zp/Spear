using System;

using Autofac.Core;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.CusException;
using Spear.Inf.Core.EncryptionNDecrypt;
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
            _cache = ServiceContext.Resolve<ICache4Redis>(new NamedPropertyParameter("redisSettings", redisSettings), new NamedPropertyParameter("defaultDatabase", _sessionNAuthSettings.CacheDBIndex));
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

                        LoadPermission();
                        VerifyPermission();

                        _curUserToken.RefreshTime = DateTime.Now;
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

        #region 鉴权初始化

        /// <summary>
        /// 加载菜单与权限
        /// </summary>
        private void LoadPermission()
        {
            try
            {
                //
            }
            catch (Exception ex)
            {
                throw new Exception("运行出错[加载菜单与权限]", ex);
            }
        }

        /// <summary>
        /// 权限验证
        /// </summary>
        private void VerifyPermission()
        {
            try
            {
                //
            }
            catch (Exception ex)
            {
                throw new Exception("运行出错[权限验证]", ex);
            }
        }

        #endregion

        #region 会话辅助

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="accessToken">Token</param>
        /// <returns></returns>
        private UserTokenCache GetUserToken(string accessToken)
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
        private void SetUserToken(UserTokenCache userToken)
        {
            if (userToken == null)
                return;

            var time = TimeSpan.FromMinutes(_sessionNAuthSettings.ValidDuration);

            var accessToken = MD5.Encrypt(userToken.AccessToken);

            _cache.Set(_sessionNAuthSettings.ValidDuration + accessToken, userToken, time);
            _curToken = userToken.AccessToken;
        }

        /// <summary>
        /// 移除Token
        /// </summary>
        /// <param name="accessToken">Token</param>
        /// <returns></returns>
        private void RemoveUserToken(string accessToken)
        {
            if (accessToken.IsEmptyString())
                return;

            accessToken = MD5.Encrypt(accessToken, 32);

            _cache.Del(_sessionNAuthSettings.CachePrefix + accessToken);
        }

        #endregion
    }
}
