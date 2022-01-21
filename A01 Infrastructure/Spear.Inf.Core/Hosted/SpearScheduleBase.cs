using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace Spear.Inf.Core.Hosted
{
    public abstract class SpearScheduleBase : BackgroundService
    {
        protected BackgroundWorker BackgroundWorker { get; private set; }

        public SpearScheduleBase()
        {
            BackgroundWorker = new BackgroundWorker();
            BackgroundWorker.DoWork += (object sender, DoWorkEventArgs e) =>
            {
                while (!ServiceContext.IsDoneLoad)
                    Thread.Sleep(1000);

                DoWork();
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            BackgroundWorker.RunWorkerAsync();
        }

        protected abstract void DoWork();
    }
}
