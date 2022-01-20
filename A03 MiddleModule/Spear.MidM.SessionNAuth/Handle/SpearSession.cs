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
    [DIModeForService(Enum_DIType.Exclusive, typeof(ISpearSession<>))]
    public class SpearSession<TTokenProvider> : ISpearSession<TTokenProvider> where TTokenProvider : ITokenProvider
    {
        private readonly SessionNAuthSettings _sessionNAuthSettings;
        private readonly ICache4Redis _cache;

        public TTokenProvider TokenProvider { get; }

        public SpearSession()
        {
            var redisSettings = ServiceContext.Resolve<RedisSettings>();

            _sessionNAuthSettings = ServiceContext.Resolve<SessionNAuthSettings>();
            //_cache = ServiceContext.Resolve<ICache4Redis>(new NamedPropertyParameter("redisSettings", redisSettings), new NamedPropertyParameter("defaultDatabase", _sessionNAuthSettings.CacheDBIndex));
            _cache = ServiceContext.Resolve<ICache4Redis>(new TypedParameter(typeof(RedisSettings), redisSettings), new TypedParameter(typeof(int), _sessionNAuthSettings.CacheDBIndex));
            TokenProvider = ServiceContext.Resolve<TTokenProvider>();
        }

        private string CurrentToken
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

        public SpearSessionInfo CurrentAccount
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

                        SpearSessionInfo userTokenCache = Get(token);
                        if (userTokenCache == null)
                            throw new Exception_NoLogin();

                        _currentUserToken = userTokenCache;

                        _currentUserToken.Extenstion(_sessionNAuthSettings.CacheMaintainMinutes);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                Set(_currentUserToken);

                return _currentUserToken;
            }
        }
        private SpearSessionInfo _currentUserToken;

        public void Set(SpearSessionInfo info)
        {
            if (info == null)
                return;

            var accessToken = info.AccessToken;
            if (_sessionNAuthSettings.AccessTokenEncrypt)
                accessToken = MD5.Encrypt(accessToken);

            _cache.Set(_sessionNAuthSettings.CachePrefix + accessToken, info, info.ExpiredTime);
        }

        public SpearSessionInfo Get(string accessToken)
        {
            if (accessToken.IsEmptyString())
                return null;

            if (_sessionNAuthSettings.AccessTokenEncrypt)
                accessToken = MD5.Encrypt(accessToken);

            return _cache.Get<SpearSessionInfo>(_sessionNAuthSettings.CachePrefix + accessToken);
        }

        public void Remove(string accessToken)
        {
            if (accessToken.IsEmptyString())
                return;

            if (_sessionNAuthSettings.AccessTokenEncrypt)
                accessToken = MD5.Encrypt(accessToken);

            _cache.Del(_sessionNAuthSettings.CachePrefix + accessToken);
        }

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

    public static class SpearSessionInfoExt
    {
        public static void SetExpiredTime(this SpearSessionInfo info)
        {
            var time = TimeSpan.FromMinutes(ServiceContext.Resolve<SessionNAuthSettings>().CacheMaintainMinutes);
            info.ExpiredTime = DateTime.Now.AddMinutes(time.TotalMinutes);
        }
    }
}
