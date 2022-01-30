using System;
using System.Threading;

namespace Spear.Inf.Core.CusResult
{
    public class ProcessAsyncResult<TData> : ProcessResult<TData>, IAsyncResult
    {
        public object AsyncState { get; protected set; }

        public WaitHandle AsyncWaitHandle { get; protected set; }

        public bool CompletedSynchronously { get; protected set; }

        public bool IsCompleted { get; protected set; }
    }
}
