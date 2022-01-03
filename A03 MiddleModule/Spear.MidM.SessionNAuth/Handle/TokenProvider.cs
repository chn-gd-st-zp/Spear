using System.Linq;

using Microsoft.AspNetCore.Http;

using Spear.Inf.Core.DTO;
using Spear.Inf.Core.ServGeneric.MicServ;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.SessionNAuth
{
    public interface ITokenProvider
    {
        string CurrentToken { get; }
    }

    public class HTTPTokenProvider : ITokenProvider
    {
        private readonly SessionNAuthSettings _sessionNAuthSettings;

        public HTTPTokenProvider(SessionNAuthSettings sessionNAuthSettings)
        {
            _sessionNAuthSettings = sessionNAuthSettings;
        }

        public string CurrentToken
        {
            get
            {
                var iContext = Spear.Inf.Core.ServGeneric.ServiceContext.Resolve<IHttpContextAccessor>();
                if (iContext == null)
                    return "";

                var context = iContext.HttpContext;
                if (context == null)
                    return "";

                var token1 = context.Request.Headers[_sessionNAuthSettings.AccessTokenKeyInHeader];
                if (token1.IsEmptyString())
                    return "";

                var token2 = token1.ToString();
                if (token2.ToLower() == "null".ToLower())
                    return "";

                return token2;
            }
        }
    }

    public class GRPCTokenProvider : ITokenProvider
    {
        public string CurrentToken
        {
            get
            {
                var context = MagicOnion.Server.ServiceContext.Current;
                if (context == null)
                    return "";

                var reqParamList = context.RestoreParams();
                if (reqParamList == null || reqParamList.Count() == 0)
                    return "";

                var baseType = typeof(IDTO_GRPC);

                var reqParam = reqParamList
                    .Where(o => o.IsExtendType(baseType))
                    .Select(o => o as IDTO_GRPC)
                    .Where(o => !o.GRPCContext.Token.IsEmptyString())
                    .FirstOrDefault();

                if (reqParam == null)
                    return "";

                return reqParam.GRPCContext.Token;
            }
        }
    }
}
