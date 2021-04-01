using System;
using System.Collections.Generic;

using Spear.Inf.Core.ServGeneric.IOC;

namespace Spear.Inf.Core.Interface
{
    public interface IRequestFilterHandle : ITransient
    {
        IRequestFilterItems FilterItems { get; }
    }

    public interface IRequestFilterItems : IList<IRequestFilterItem>, ITransient
    {
        //
    }

    public interface IRequestFilterItem : ITransient
    {
        string Entrance { get; set; }
        string Action { get; set; }
        object Header { get; set; }
        object ReqParams { get; set; }
        object FuncParams { get; set; }
        object Result { get; set; }
        Exception Exception { get; set; }
        DateTime? RequestTime { get; set; }
        DateTime? ResponseTime { get; set; }

        void OnExecuting(object context);

        void OnExecuted(object context);

        void OnExit(object context);

        void OnException(object context, Exception exception);
    }
}
