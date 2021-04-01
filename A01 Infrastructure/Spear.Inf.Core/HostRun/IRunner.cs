using System.Threading;
using System.Threading.Tasks;

using Spear.Inf.Core.CusEnum;

namespace Spear.Inf.Core
{
    public interface IRunner
    {
        /// <summary>
        /// 运行状态
        /// </summary>
        Enum_Process RunningStatus { get; }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Run(CancellationToken cancellationToken = default);

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Dispose(CancellationToken cancellationToken = default);
    }
}
