using System.Linq;

using Microsoft.AspNetCore.Http;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric.MicServ;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.SessionNAuth
{
    public abstract class TokenProvider : ITokenProvider
    {
        protected readonly SessionNAuthSettings SessionNAuthSettings;

        public TokenProvider() { SessionNAuthSettings = Inf.Core.ServGeneric.ServiceContext.Resolve<SessionNAuthSettings>(); }

        public virtual Enum_Protocol Protocol { get { return Enum_Protocol.None; } }

        public virtual string CurrentToken { get { return ""; } }
    }

    [DIModeForService(Enum_DIType.AsSelf)]
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(ITokenProvider), Enum_Protocol.HTTP)]
    public class HTTPTokenProvider : TokenProvider
    {
        public override Enum_Protocol Protocol { get { return Enum_Protocol.HTTP; } }

        public override string CurrentToken
        {
            get
            {
                var iContext = Inf.Core.ServGeneric.ServiceContext.Resolve<IHttpContextAccessor>();
                if (iContext == null)
                    return "";

                var context = iContext.HttpContext;
                if (context == null)
                    return "";

                var token1 = context.Request.Headers[SessionNAuthSettings.AccessTokenKeyInHeader];
                if (token1.IsEmptyString())
                    return "";

                var token2 = token1.ToString();
                if (token2.ToLower() == "null".ToLower())
                    return "";

                return token2;
            }
        }
    }

    [DIModeForService(Enum_DIType.AsSelf)]
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(ITokenProvider), Enum_Protocol.GRPC)]
    public class GRPCTokenProvider : TokenProvider
    {
        public override Enum_Protocol Protocol { get { return Enum_Protocol.GRPC; } }

        public override string CurrentToken
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
