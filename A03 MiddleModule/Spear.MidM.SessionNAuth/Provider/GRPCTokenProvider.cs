using System.Linq;

using MagicOnion.Server;

using Spear.Inf.Core.DTO;
using Spear.Inf.Core.ServGeneric.MicServ;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.SessionNAuth
{
    public class GRPCTokenProvider : ITokenProvider
    {
        public string CurToken
        {
            get
            {
                var context = ServiceContext.Current;
                if (context == null)
                    return "";

                var reqParamList = context.RestoreParams();
                if (reqParamList == null || reqParamList.Count() == 0)
                    return "";

                var basicType = typeof(IDTO_GRPC);

                var reqParam = reqParamList
                    .Where(o => o.IsExtendType(basicType))
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
