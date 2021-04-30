using System;

using Spear.Inf.Core.CusResult;
using Spear.Demo4GRPC.Pub.Basic;

namespace Spear.Demo4GRPC.Host.Server.Implement
{
    public class Analyzer : IAnalyzer
    {
        public Analyzer()
        {
            //
        }

        public ResultBasic<bool> Run()
        {
            Console.WriteLine("解析数据开始");
            Console.WriteLine("解析数据完成");
            Console.WriteLine("发布消息队列 - 解析数据完成");

            return true.ResultBasic_Success();
        }
    }
}
