using System;

namespace Spear.Inf.Core.Interface
{
    public interface ISpearLogger
    {
        void Info(string msg);

        void Info<T>(T obj);

        void Error(string msg, Exception exception = null);

        void Error<T>(T obj, Exception exception = null);
    }

    public interface ISpearLogger<TTrigger> : ISpearLogger where TTrigger : class { }
}
