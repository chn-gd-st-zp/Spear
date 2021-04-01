using System;

namespace Spear.Inf.Core.Interface
{
    public interface ILogger
    {
        void Info(string msg);

        void Info(object obj);

        void Error(string msg);

        void Error(object obj);

        void Error(string msg, Exception exception);

        void Error(object obj, Exception exception);
    }

    public interface ILogger<T> : ILogger
        where T : class
    {
        //
    }
}
