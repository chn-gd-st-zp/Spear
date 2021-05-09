using System;

using Microsoft.Extensions.Logging;

using NLog;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.ServGeneric.IOC;
using Spear.Inf.Core.Tool;

using CUS = Spear.Inf.Core.Interface;

namespace Spear.MidM.Logger
{
    [DIModeForService(Enum_DIType.Specific, typeof(CUS.ILogger))]
    public class NLogger : CUS.ILogger, ITransient
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

        public void Info(object obj)
        {
            Info(obj.ToJson());
        }

        public void Error(string msg, Exception exception = null)
        {
            exception = exception != null ? exception : new Exception();
            var errorObj = new { ErrorMsg = msg, ErrorInfo = exception.Message, ErrorTrace = exception.StackTrace };
            _logger.Error(exception, errorObj.ToJson());
        }

        public void Error(object obj, Exception exception = null)
        {
            exception = exception != null ? exception : new Exception();
            var errorObj = new { ErrorObj = obj, ErrorInfo = exception.Message, ErrorTrace = exception.StackTrace };
            _logger.Error(exception, errorObj.ToJson());
        }
    }

    public class NLogger<T> : CUS.ILogger<T>
        where T : class
    {
        private ILogger<T> _logger;

        public NLogger()
        {
            _logger = ServiceContext.Resolve<ILogger<T>>();
        }

        public void Info(string msg)
        {
            _logger.LogInformation(msg);
        }

        public void Info(object obj)
        {
            Info(obj.ToJson());
        }

        public void Error(string msg, Exception exception = null)
        {
            exception = exception != null ? exception : new Exception();
            var errorObj = new { ErrorMsg = msg, ErrorInfo = exception.Message, ErrorTrace = exception.StackTrace };
            _logger.LogError(exception, errorObj.ToJson());
        }

        public void Error(object obj, Exception exception = null)
        {
            exception = exception != null ? exception : new Exception();
            var errorObj = new { ErrorObj = obj, ErrorInfo = exception.Message, ErrorTrace = exception.StackTrace };
            _logger.LogError(exception, errorObj.ToJson());
        }
    }
}
