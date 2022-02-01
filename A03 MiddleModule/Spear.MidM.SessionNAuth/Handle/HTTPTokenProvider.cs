using Microsoft.AspNetCore.Http;

using Spear.Inf.Core;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.SessionNAuth
{
    [DIModeForService(Enum_DIType.AsSelf)]
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(ITokenProvider), Enum_Protocol.HTTP)]
    public class HTTPTokenProvider : ITokenProvider
    {
        public Enum_Protocol Protocol { get { return Enum_Protocol.HTTP; } }

        public string CurrentToken
        {
            get
            {
                var iContext = ServiceContext.Resolve<IHttpContextAccessor>();
                if (iContext == null)
                    return string.Empty;

                var context = iContext.HttpContext;
                if (context == null)
                    return string.Empty;

                var sessionNAuthSettings = ServiceContext.Resolve<SessionNAuthSettings>();
                var token1 = context.Request.Headers[sessionNAuthSettings.AccessTokenKeyInHeader];
                if (token1.IsEmptyString())
                    return string.Empty;

                var token2 = token1.ToString();
                if (token2.ToLower() == "null".ToLower())
                    return string.Empty;

                return token2;
            }
        }
    }

    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(ISpearSession), Enum_Protocol.HTTP)]
    public class SpearSession4HTTP : SpearSession<HTTPTokenProvider> { }
}
