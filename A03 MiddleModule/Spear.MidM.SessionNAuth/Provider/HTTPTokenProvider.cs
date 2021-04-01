using Microsoft.AspNetCore.Http;

using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.SessionNAuth
{
    public class HTTPTokenProvider : ITokenProvider
    {
        private readonly SessionNAuthSettings _sessionNAuthSettings;

        public HTTPTokenProvider(SessionNAuthSettings sessionNAuthSettings)
        {
            _sessionNAuthSettings = sessionNAuthSettings;
        }

        public string CurToken
        {
            get
            {
                var iContext = ServiceContext.ResolveServ<IHttpContextAccessor>();
                if (iContext == null)
                    return "";

                var context = iContext.HttpContext;
                if (context == null)
                    return "";

                var token1 = context.Request.Headers[_sessionNAuthSettings.TokenKey];
                if (token1.IsEmptyString())
                    return "";

                var token2 = token1.ToString();
                if (token2.ToLower() == "null".ToLower())
                    return "";

                return token2;
            }
        }
    }
}
