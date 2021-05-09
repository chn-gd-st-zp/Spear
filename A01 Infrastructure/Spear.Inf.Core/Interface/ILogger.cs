using System;

namespace Spear.Inf.Core.Interface
{
    public interface ILogger
    {
        void Info(string msg);

        void Info(object obj);

        void Error(string msg, Exception exception = null);

        void Error(object obj, Exception exception = null);
    }

    public interface ILogger<T> : ILogger
        where T : class
    {
        //
    }
}
