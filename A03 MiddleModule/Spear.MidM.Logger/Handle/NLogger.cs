using System;

using Microsoft.Extensions.Logging;

using NLog;

using Spear.Inf.Core;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Logger
{
    public interface INLogger : ISpearLogger { }

    public interface INLogger<TTrigger> : INLogger, ISpearLogger<TTrigger> where TTrigger : class { }

    public class NLogger : INLogger
    {
        private NLog.ILogger _logger;

        public NLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Info(string msg)
        {
            _logger.Info(msg);
        }

        public void Info<T>(T obj)
        {
            Info(obj.ToJson());
        }

        public void Error(Exception exception)
        {
            _logger.Error(exception);
        }

        public void Error(string msg, Exception exception = null)
        {
            exception = exception != null ? exception : new Exception();
            var errorObj = new { ErrorMsg = msg, ErrorInfo = exception.Message, ErrorTrace = exception.StackTrace };
            _logger.Error(exception, errorObj.ToJson());
        }

        public void Error<T>(T obj, Exception exception = null)
        {
            exception = exception != null ? exception : new Exception();
            var errorObj = new { ErrorObj = obj, ErrorInfo = exception.Message, ErrorTrace = exception.StackTrace };
            _logger.Error(exception, errorObj.ToJson());
        }
    }

    public class NLogger<TTrigger> : INLogger<TTrigger>
        where TTrigger : class
    {
        private ILogger<TTrigger> _logger;

        public NLogger()
        {
            _logger = ServiceContext.Resolve<ILogger<TTrigger>>();
        }

        public void Info(string msg)
        {
            _logger.LogInformation(msg);
        }

        public void Info<T>(T obj)
        {
            Info(obj.ToJson());
        }

        public void Error(Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }

        public void Error(string msg, Exception exception = null)
        {
            exception = exception != null ? exception : new Exception();
            var errorObj = new { ErrorMsg = msg, ErrorInfo = exception.Message, ErrorTrace = exception.StackTrace };
            _logger.LogError(exception, errorObj.ToJson());
        }

        public void Error<T>(T obj, Exception exception = null)
        {
            exception = exception != null ? exception : new Exception();
            var errorObj = new { ErrorObj = obj, ErrorInfo = exception.Message, ErrorTrace = exception.StackTrace };
            _logger.LogError(exception, errorObj.ToJson());
        }
    }
}
