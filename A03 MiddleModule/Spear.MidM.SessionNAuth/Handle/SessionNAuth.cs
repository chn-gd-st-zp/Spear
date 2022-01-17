using System;
using System.Linq;

using Autofac;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.CusException;
using Spear.Inf.Core.EncryptionNDecrypt;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.Tool;
using Spear.MidM.Redis;

namespace Spear.MidM.SessionNAuth
{
    public interface ISessionNAuth : ISession
    {
        string CurrentToken { get; }

        UserTokenRunTime CurrentAccount { get; }

        void SetUserToken(UserTokenCache userToken);

        UserTokenCache GetUserToken(string accessToken);

        void RemoveUserToken(string accessToken);

        void VerifyPermission(string permissionCode);
    }

    public interface ISessionNAuth<TTokenProvider> : ISession<TTokenProvider>, ISessionNAuth where TTokenProvider : ITokenProvider { }

    [DIModeForService(Enum_DIType.Exclusive, typeof(ISession<>))]
    [DIModeForService(Enum_DIType.Exclusive, typeof(ISessionNAuth<>))]
    public class SessionNAuth<TTokenProvider> : ISessionNAuth<TTokenProvider> where TTokenProvider : ITokenProvider
    {
        private readonly SessionNAuthSettings _sessionNAuthSettings;
        private readonly ICache4Redis _cache;

        public TTokenProvider TokenProvider { get; }

        public SessionNAuth()
        {
            var redisSettings = ServiceContext.Resolve<RedisSettings>();

            _sessionNAuthSettings = ServiceContext.Resolve<SessionNAuthSettings>();
            //_cache = ServiceContext.Resolve<ICache4Redis>(new NamedPropertyParameter("redisSettings", redisSettings), new NamedPropertyParameter("defaultDatabase", _sessionNAuthSettings.CacheDBIndex));
            _cache = ServiceContext.Resolve<ICache4Redis>(new TypedParameter(typeof(RedisSettings), redisSettings), new TypedParameter(typeof(int), _sessionNAuthSettings.CacheDBIndex));
            TokenProvider = ServiceContext.Resolve<TTokenProvider>();
        }

        /// <summary>
        /// 当前Token(字符串)
        /// </summary>
        public string CurrentToken
        {
            get
            {
                try
                {
                    if (_currentToken.IsEmptyString())
                    {
                        var token1 = TokenProvider.CurrentToken;
                        var token2 = token1.IsEmptyString() ? "" : token1;
                        var token = token2.ToLower() == "null" ? "" : token2;
                        _currentToken = token;
                    }

                    return _currentToken;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private string _currentToken;

        /// <summary>
        /// 当前会话对象
        /// </summary>
        public UserTokenRunTime CurrentAccount
        {
            get
            {
                if (_currentUserToken == null)
                {
                    try
                    {
                        string token = CurrentToken;
                        if (token.IsEmptyString())
                            throw new Exception_EmptyToken();

                        UserTokenCache userTokenCache = GetUserToken(token);
                        if (userTokenCache == null)
                            throw new Exception_NoLogin();

                        _currentUserToken = userTokenCache.MapTo<UserTokenCache, UserTokenRunTime>();

                        _currentUserToken.Extenstion(_sessionNAuthSettings.CacheMaintainMinutes);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                SetUserToken(_currentUserToken.MapTo<UserTokenRunTime, UserTokenCache>());

                return _currentUserToken;
            }
        }
        private UserTokenRunTime _currentUserToken;

        /// <summary>
        /// 设置Token
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public void SetUserToken(UserTokenCache userToken)
        {
            if (userToken == null)
                return;

            var time = TimeSpan.FromMinutes(_sessionNAuthSettings.CacheMaintainMinutes);
            userToken.ExpiredTime = DateTime.Now.AddMinutes(time.TotalMinutes);

            var accessToken = userToken.AccessToken;
            if (_sessionNAuthSettings.AccessTokenEncrypt)
                accessToken = MD5.Encrypt(accessToken);

            _cache.Set(_sessionNAuthSettings.CachePrefix + accessToken, userToken, time);
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="accessToken">Token</param>
        /// <returns></returns>
        public UserTokenCache GetUserToken(string accessToken)
        {
            if (accessToken.IsEmptyString())
                return null;

            if (_sessionNAuthSettings.AccessTokenEncrypt)
                accessToken = MD5.Encrypt(accessToken);

            return _cache.Get<UserTokenCache>(_sessionNAuthSettings.CachePrefix + accessToken);
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

            if (_sessionNAuthSettings.AccessTokenEncrypt)
                accessToken = MD5.Encrypt(accessToken);

            _cache.Del(_sessionNAuthSettings.CachePrefix + accessToken);
        }

        /// <summary>
        /// 权限认证
        /// </summary>
        public void VerifyPermission(string permissionCode)
        {
            try
            {
                if (CurrentAccount.AccountInfo.ERoleType == Enum_Role.SuperAdmin)
                    return;

                if (CurrentAccount.PermissionCodes.Contains(permissionCode))
                    return;

                throw new Exception_NoAuth();
            }
            catch (Exception ex)
            {
                throw new Exception("运行出错[权限认证]", ex);
            }
        }
    }

    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(ISessionNAuth), Enum_Protocol.HTTP)]
    public class SessionNAuth4HTTP : SessionNAuth<HTTPTokenProvider> { }

    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(ISessionNAuth), Enum_Protocol.GRPC)]
    public class SessionNAuth4GRPC : SessionNAuth<GRPCTokenProvider> { }
}
