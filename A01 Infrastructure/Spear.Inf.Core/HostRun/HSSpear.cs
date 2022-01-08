﻿using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core
{
    public class HSSpear : IHostedService
    {
        protected IRSpear Runner;

        protected BackgroundWorker BackgroundWorker { get; private set; }

        public HSSpear(IRSpear runner)
        {
            Runner = runner;
            BackgroundWorker = new BackgroundWorker();
            BackgroundWorker.DoWork += (object sender, DoWorkEventArgs e) => { Runner.Run((CancellationToken)e.Argument); };
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            BackgroundWorker.RunWorkerAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Runner.Dispose(cancellationToken);
            BackgroundWorker.Dispose();
        }
    }

    public class HSSpear<THSSpear> : HSSpear
        where THSSpear : IRSpear
    {
        public HSSpear(THSSpear runner) : base(runner) { }
    }

    public abstract class HSSpearScheduleBase : BackgroundService
    {
        protected BackgroundWorker BackgroundWorker { get; private set; }

        public HSSpearScheduleBase()
        {
            BackgroundWorker = new BackgroundWorker();
            BackgroundWorker.DoWork += (object sender, DoWorkEventArgs e) =>
            {
                while (!ServiceContext.IsDoneLoad)
                    Thread.Sleep(1000);

                DoWork();
            };
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            BackgroundWorker.RunWorkerAsync();
            return Task.CompletedTask;
        }

        protected abstract void DoWork();
    }
}