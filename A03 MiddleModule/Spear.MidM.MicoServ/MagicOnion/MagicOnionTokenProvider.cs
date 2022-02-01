using System.Linq;

using MagicOnion.Server;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;
using Spear.MidM.SessionNAuth;

namespace Spear.MidM.MicoServ.MagicOnion
{
    [DIModeForService(Enum_DIType.AsSelf)]
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(ITokenProvider), Enum_Protocol.GRPC)]
    public class GRPCTokenProvider : ITokenProvider
    {
        public Enum_Protocol Protocol { get { return Enum_Protocol.GRPC; } }

        public string CurrentToken
        {
            get
            {
                var context = ServiceContext.Current;
                if (context == null)
                    return string.Empty;

                var reqParamList = context.RestoreParams();
                if (reqParamList == null || reqParamList.Count() == 0)
                    return string.Empty;

                var baseType = typeof(IDTO_GRPC);

                var reqParam = reqParamList
                    .Where(o => o.GetType().IsExtendOf(baseType))
                    .Select(o => o as IDTO_GRPC)
                    .Where(o => !o.GRPCContext.Token.IsEmptyString())
                    .FirstOrDefault();

                if (reqParam == null)
                    return string.Empty;

                return reqParam.GRPCContext.Token;
            }
        }
    }

    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(ISpearSession), Enum_Protocol.GRPC)]
    public class SpearSession4GRPC : SpearSession<GRPCTokenProvider> { }
}
