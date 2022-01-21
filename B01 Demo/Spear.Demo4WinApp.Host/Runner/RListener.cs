using System;
using System.Threading;
using System.Threading.Tasks;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Injection;
using Spear.MidM.Schedule;

namespace Spear.Demo4WinApp.Host.Runner
{
    [DIModeForService(Enum_DIType.ExclusiveByNamed, typeof(IRunner4BGWorker), "Listener")]
    public class RListener : IRunner4BGWorker, ITransient
    {
        public async Task Run(params string[] args)
        {
            while (true)
            {
                Console.WriteLine($"Listener:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

                Thread.Sleep(1000 * int.Parse(args[0]));
            }
        }
    }
}
