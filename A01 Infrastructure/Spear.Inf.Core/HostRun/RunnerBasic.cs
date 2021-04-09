using System;
using System.Threading;
using System.Threading.Tasks;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core
{
    public delegate void NewTaskDelegate(params object[] paramArray);

    public abstract class RunnerBasic<TRunner> : IRunner
        where TRunner : class
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected ILogger<TRunner> Logger { get; private set; }

        /// <summary>
        /// 运行状态
        /// </summary>
        public Enum_Process RunningStatus
        {
            get { return _runningStatus; }
            protected set
            {
                if (_runningStatus == value)
                    return;

                UpdateStatus(value);

                _runningStatus = value;
            }
        }
        private Enum_Process _runningStatus = Enum_Process.None;

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="eProcess"></param>
        protected virtual void UpdateStatus(Enum_Process eProcess) { }

        /// <summary>
        /// 初始化
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Run(CancellationToken cancellationToken)
        {
            string runnerName = typeof(TRunner).Name;

            Info($"[{runnerName}]管道初始化...");
            while (ServiceContext.IsDoneLoad) Thread.Sleep(1000);
            Info($"[{runnerName}]管道初始化完成...");

            Info($"[{runnerName}]程序初始化...");
            Logger = ServiceContext.Resolve<ILogger<TRunner>>();
            RunningStatus = Enum_Process.Waiting;
            Init();
            Info($"[{runnerName}]程序初始化完成...");

            Info($"[{runnerName}]程序开始运行...");
            Execute(cancellationToken);
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task Dispose(CancellationToken cancellationToken);

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract Task Execute(CancellationToken cancellationToken);

        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="text"></param>
        protected void Info(string text)
        {
            Printor.PrintText(text);

            if (Logger != null)
                Logger.Info(text);
        }
    }
}
