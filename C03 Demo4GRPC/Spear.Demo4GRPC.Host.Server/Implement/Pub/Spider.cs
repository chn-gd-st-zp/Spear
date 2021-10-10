using System;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.ServGeneric;
using Spear.Demo4GRPC.Pub.Base;

namespace Spear.Demo4GRPC.Host.Server.Implement
{
    public class Spider : ISpider
    {
        private readonly IAnalyzer _analyzer;

        public Spider()
        {
            _analyzer = ServiceContext.Resolve<IAnalyzer>();
        }

        ResultBase<bool> ISpider.Run()
        {
            Console.WriteLine("抓数据开始");
            Console.WriteLine("抓数据完成");

            //Console.WriteLine("发布消息队列 - 抓数据完成");
            Console.WriteLine("调用解析服务");

            _analyzer.Run();

            return true.ResultBase_Success();
        }
    }
}
