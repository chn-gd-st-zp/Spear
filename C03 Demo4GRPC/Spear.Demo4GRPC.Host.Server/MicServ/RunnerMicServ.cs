using MagicOnion;
using MagicOnion.Server;

using Spear.Inf.Core.CusResult;
using Spear.GlobalSupport.Base.Interface;
using Spear.Demo4GRPC.Pub.Base;

using ServiceContext = Spear.Inf.Core.ServGeneric.ServiceContext;

namespace Spear.Demo4GRPC.Host.Server.MicServ
{
    public class RunnerMicServ : ServiceBase<IMSRunner>, IMSRunner
    {
        private readonly IDataProvider _dataProvider;

        public RunnerMicServ()
        {
            _dataProvider = ServiceContext.Resolve<IDataProvider>();
        }

        public UnaryResult<ResultMicServ<bool>> Run(params string[] args)
        {
            var execResult = new ResultBase<bool>();

            execResult = _dataProvider.Spider.Run();
            if (!execResult.IsSuccess)
                return execResult.ToMicServResult();

            execResult = _dataProvider.Analyzer.Run();
            if (!execResult.IsSuccess)
                return execResult.ToMicServResult();

            return true.ResultBase_Success().ToMicServResult();
        }
    }
}
