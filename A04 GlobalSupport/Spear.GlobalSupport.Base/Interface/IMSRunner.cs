using MagicOnion;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.ServGeneric.MicServ;

namespace Spear.GlobalSupport.Base.Interface
{
    public interface IMSRunner : IMicServ<IMSRunner>
    {
        UnaryResult<ResultMicServ<bool>> Run(params string[] args);
    }
}
