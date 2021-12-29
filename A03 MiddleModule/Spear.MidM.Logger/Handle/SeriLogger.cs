using System;
using System.Collections.Generic;
using System.Linq;

using Serilog;
using Serilog.Core;
using Serilog.Configuration;
using Serilog.Events;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.SettingsGeneric;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Logger
{
    [DIModeForSettings("SeriLogSettings", typeof(SeriLogSettings))]
    public class SeriLogSettings : ISettings
    {
        public RollingInterval RollingInterval { get; set; }
        public bool RollOnFileSizeLimit { get; set; }
        public int FileSizeLimitMB { get; set; }
        public string Template { get; set; }
        public SeriLogToEmail ToEmail { get; set; }
    }

    public class SeriLogToEmail
    {
        public bool Enable { get; set; }
        public LogEventLevel MinimumLevel { get; set; }
        public string[] Triggers { get; set; }
        public string MailServer { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
        public string[] CarbonCopyEmail { get; set; }
        public string[] ReceiverEmails { get; set; }
    }

    public static class SeriLoggerExtend
    {
        public static LoggerConfiguration ToFile<TTrigger>(this LoggerConfiguration configuration, LogEventLevel level)
        {
            var settings = ServiceContext.Resolve<SeriLogSettings>();
            var triggerName = typeof(TTrigger).Name;

            return configuration
                .WriteTo.File(
                    path: $"Logs/{triggerName}/{level}-.log",
                    restrictedToMinimumLevel: level,
                    outputTemplate: settings.Template,
                    rollingInterval: settings.RollingInterval,
                    rollOnFileSizeLimit: settings.RollOnFileSizeLimit,
                    fileSizeLimitBytes: settings.FileSizeLimitMB * 1024 * 1024,
                    //retainedFileCountLimit: 7,
                    encoding: System.Text.Encoding.UTF8
                );
        }

        public static LoggerConfiguration InclusiveLogger<TTrigger>(this LoggerSinkConfiguration configuration, LogEventLevel level)
        {
            var triggerName = typeof(TTrigger).Name;
            return configuration
                .Logger(o =>
                    o.Filter
                    .ByIncludingOnly(e => e.Level == level)
                    .ToFile<TTrigger>(level)
                );
        }

        public static void ToEmail<TTrigger>(this SeriLogger<TTrigger> seriLogger, LogEventLevel level, string message, Exception exception = null)
            where TTrigger : class
        {
            var settings = ServiceContext.Resolve<SeriLogSettings>();
            var triggerName = typeof(TTrigger).Name;

            if (!settings.ToEmail.Enable)
                return;

            if (settings.ToEmail.MinimumLevel > level)
                return;

            if (!settings.ToEmail.Triggers.Contains(triggerName))
                return;

            string realMsg = "";
            string pattern = settings.Template;
            do
            {
                if (pattern.StartsWith(" "))
                {
                    realMsg += " ";
                    pattern = pattern.Substring(1);
                    continue;
                }

                pattern = pattern.Substring(pattern.IndexOf("{") + 1);

                if (pattern.StartsWith("NewLine", StringComparison.OrdinalIgnoreCase))
                {
                    realMsg += @"<br>";
                    pattern = pattern.Substring(pattern.IndexOf("}") + 1);
                    continue;
                }

                if (pattern.StartsWith("TimeStamp:", StringComparison.OrdinalIgnoreCase))
                {
                    string format = pattern.Substring(pattern.IndexOf(":") + 1, pattern.IndexOf("}") - 1 - pattern.IndexOf(":"));

                    realMsg += DateTime.Now.ToString(format);
                    pattern = pattern.Substring(pattern.IndexOf("}") + 1);
                    continue;
                }

                if (pattern.StartsWith("Message", StringComparison.OrdinalIgnoreCase))
                {
                    realMsg += message;
                    pattern = pattern.Substring(pattern.IndexOf("}") + 1);
                    continue;
                }

                if (pattern.StartsWith("Exception", StringComparison.OrdinalIgnoreCase))
                {
                    realMsg += exception == null ? "" : exception.ToString();
                    pattern = pattern.Substring(pattern.IndexOf("}") + 1);
                    continue;
                }

            } while (!pattern.IsEmptyString());

            //send email;
        }
    }

    public interface ISeriLogger<TTrigger> : ISpearLogger<TTrigger>, ILogger where TTrigger : class { }

    public class SeriLogger<TTrigger> : ISeriLogger<TTrigger>
        where TTrigger : class
    {
        private ILogger _logger { get; set; }

        public SeriLogger()
        {
            _logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.InclusiveLogger<TTrigger>(LogEventLevel.Verbose)
                .WriteTo.InclusiveLogger<TTrigger>(LogEventLevel.Debug)
                .WriteTo.InclusiveLogger<TTrigger>(LogEventLevel.Information)
                .WriteTo.InclusiveLogger<TTrigger>(LogEventLevel.Warning)
                .WriteTo.InclusiveLogger<TTrigger>(LogEventLevel.Error)
                .WriteTo.InclusiveLogger<TTrigger>(LogEventLevel.Fatal)
                .CreateLogger();
        }

        public bool IsEnabled(LogEventLevel level)
        { return _logger.IsEnabled(level); }

        #region Bind

        public bool BindMessageTemplate(string messageTemplate, object[] propertyValues, out MessageTemplate parsedTemplate, out IEnumerable<LogEventProperty> boundProperties)
        { return _logger.BindMessageTemplate(messageTemplate, propertyValues, out parsedTemplate, out boundProperties); }

        public bool BindProperty(string propertyName, object value, bool destructureObjects, out LogEventProperty property)
        { return _logger.BindProperty(propertyName, value, destructureObjects, out property); }

        #endregion

        #region ForContext

        public ILogger ForContext(ILogEventEnricher enricher)
        { return _logger.ForContext(enricher); }

        public ILogger ForContext(IEnumerable<ILogEventEnricher> enrichers)
        { return _logger.ForContext(enrichers); }

        public ILogger ForContext(string propertyName, object value, bool destructureObjects = false)
        { return _logger.ForContext(propertyName, value, destructureObjects); }

        public ILogger ForContext<TSource>()
        { return _logger.ForContext<TSource>(); }

        public ILogger ForContext(Type source)
        { return _logger.ForContext(source); }

        #endregion

        #region Write

        public void Write(LogEvent logEvent)
        { _logger.Write(logEvent); this.ToEmail(logEvent.Level, logEvent.MessageTemplate.Text); }

        public void Write(LogEventLevel level, string messageTemplate)
        { _logger.Write(level, messageTemplate); this.ToEmail(level, messageTemplate); }

        public void Write(LogEventLevel level, string messageTemplate, params object[] propertyValues)
        { _logger.Write(level, messageTemplate, propertyValues); this.ToEmail(level, messageTemplate); }

        public void Write<T>(LogEventLevel level, string messageTemplate, T propertyValue)
        { _logger.Write(level, messageTemplate, propertyValue); this.ToEmail(level, messageTemplate); }

        public void Write<T0, T1>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        { _logger.Write(level, messageTemplate, propertyValue0, propertyValue1); this.ToEmail(level, messageTemplate); }

        public void Write<T0, T1, T2>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        { _logger.Write(level, messageTemplate, propertyValue0, propertyValue1, propertyValue2); this.ToEmail(level, messageTemplate); }

        public void Write(LogEventLevel level, Exception exception, string messageTemplate)
        { _logger.Write(level, exception, messageTemplate); this.ToEmail(level, messageTemplate, exception); }

        public void Write(LogEventLevel level, Exception exception, string messageTemplate, params object[] propertyValues)
        { _logger.Write(level, exception, messageTemplate, propertyValues); this.ToEmail(level, messageTemplate, exception); }

        public void Write<T>(LogEventLevel level, Exception exception, string messageTemplate, T propertyValue)
        { _logger.Write(level, exception, messageTemplate, propertyValue); this.ToEmail(level, messageTemplate, exception); }

        public void Write<T0, T1>(LogEventLevel level, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        { _logger.Write(level, exception, messageTemplate, propertyValue0, propertyValue1); this.ToEmail(level, messageTemplate, exception); }

        public void Write<T0, T1, T2>(LogEventLevel level, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        { _logger.Write(level, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2); this.ToEmail(level, messageTemplate, exception); }

        #endregion

        #region Verbose

        public void Verbose(string messageTemplate)
        { _logger.Verbose(messageTemplate); this.ToEmail(LogEventLevel.Verbose, messageTemplate); }

        public void Verbose(string messageTemplate, params object[] propertyValues)
        { _logger.Verbose(messageTemplate, propertyValues); this.ToEmail(LogEventLevel.Verbose, messageTemplate); }

        public void Verbose<T>(string messageTemplate, T propertyValue)
        { _logger.Verbose(messageTemplate, propertyValue); this.ToEmail(LogEventLevel.Verbose, messageTemplate); }

        public void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        { _logger.Verbose(messageTemplate, propertyValue0, propertyValue1); this.ToEmail(LogEventLevel.Verbose, messageTemplate); }

        public void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        { _logger.Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue2); this.ToEmail(LogEventLevel.Verbose, messageTemplate); }

        public void Verbose(Exception exception, string messageTemplate)
        { _logger.Verbose(exception, messageTemplate); this.ToEmail(LogEventLevel.Verbose, messageTemplate, exception); }

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        { _logger.Verbose(exception, messageTemplate, propertyValues); this.ToEmail(LogEventLevel.Verbose, messageTemplate, exception); }

        public void Verbose<T>(Exception exception, string messageTemplate, T propertyValue)
        { _logger.Verbose(exception, messageTemplate, propertyValue); this.ToEmail(LogEventLevel.Verbose, messageTemplate, exception); }

        public void Verbose<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        { _logger.Verbose(exception, messageTemplate, propertyValue0, propertyValue1); this.ToEmail(LogEventLevel.Verbose, messageTemplate, exception); }

        public void Verbose<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        { _logger.Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2); this.ToEmail(LogEventLevel.Verbose, messageTemplate, exception); }

        #endregion

        #region Debug

        public void Debug(string messageTemplate)
        { _logger.Debug(messageTemplate); this.ToEmail(LogEventLevel.Debug, messageTemplate); }

        public void Debug(string messageTemplate, params object[] propertyValues)
        { _logger.Debug(messageTemplate, propertyValues); this.ToEmail(LogEventLevel.Debug, messageTemplate); }

        public void Debug<T>(string messageTemplate, T propertyValue)
        { _logger.Debug(messageTemplate, propertyValue); this.ToEmail(LogEventLevel.Debug, messageTemplate); }

        public void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        { _logger.Debug(messageTemplate, propertyValue0, propertyValue1); this.ToEmail(LogEventLevel.Debug, messageTemplate); }

        public void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        { _logger.Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2); this.ToEmail(LogEventLevel.Debug, messageTemplate); }

        public void Debug(Exception exception, string messageTemplate)
        { _logger.Debug(exception, messageTemplate); this.ToEmail(LogEventLevel.Debug, messageTemplate, exception); }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        { _logger.Debug(exception, messageTemplate, propertyValues); this.ToEmail(LogEventLevel.Debug, messageTemplate, exception); }

        public void Debug<T>(Exception exception, string messageTemplate, T propertyValue)
        { _logger.Debug(exception, messageTemplate, propertyValue); this.ToEmail(LogEventLevel.Debug, messageTemplate, exception); }

        public void Debug<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        { _logger.Debug(exception, messageTemplate, propertyValue0, propertyValue1); this.ToEmail(LogEventLevel.Debug, messageTemplate, exception); }

        public void Debug<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        { _logger.Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2); this.ToEmail(LogEventLevel.Debug, messageTemplate, exception); }

        #endregion

        #region Information

        public void Information(string messageTemplate)
        { _logger.Information(messageTemplate); this.ToEmail(LogEventLevel.Information, messageTemplate); }

        public void Information(string messageTemplate, params object[] propertyValues)
        { _logger.Information(messageTemplate, propertyValues); this.ToEmail(LogEventLevel.Information, messageTemplate); }

        public void Information<T>(string messageTemplate, T propertyValue)
        { _logger.Information(messageTemplate, propertyValue); this.ToEmail(LogEventLevel.Information, messageTemplate); }

        public void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        { _logger.Information(messageTemplate, propertyValue0, propertyValue1); this.ToEmail(LogEventLevel.Information, messageTemplate); }

        public void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        { _logger.Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2); this.ToEmail(LogEventLevel.Information, messageTemplate); }

        public void Information(Exception exception, string messageTemplate)
        { _logger.Information(exception, messageTemplate); this.ToEmail(LogEventLevel.Information, messageTemplate, exception); }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        { _logger.Information(exception, messageTemplate, propertyValues); this.ToEmail(LogEventLevel.Information, messageTemplate, exception); }

        public void Information<T>(Exception exception, string messageTemplate, T propertyValue)
        { _logger.Information(exception, messageTemplate, propertyValue); this.ToEmail(LogEventLevel.Information, messageTemplate, exception); }

        public void Information<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        { _logger.Information(exception, messageTemplate, propertyValue0, propertyValue1); this.ToEmail(LogEventLevel.Information, messageTemplate, exception); }

        public void Information<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        { _logger.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2); this.ToEmail(LogEventLevel.Information, messageTemplate, exception); }

        #endregion

        #region Warning

        public void Warning(string messageTemplate)
        { _logger.Warning(messageTemplate); this.ToEmail(LogEventLevel.Warning, messageTemplate); }

        public void Warning(string messageTemplate, params object[] propertyValues)
        { _logger.Warning(messageTemplate, propertyValues); this.ToEmail(LogEventLevel.Warning, messageTemplate); }

        public void Warning<T>(string messageTemplate, T propertyValue)
        { _logger.Warning(messageTemplate, propertyValue); this.ToEmail(LogEventLevel.Warning, messageTemplate); }

        public void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        { _logger.Warning(messageTemplate, propertyValue0, propertyValue1); this.ToEmail(LogEventLevel.Warning, messageTemplate); }

        public void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        { _logger.Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2); this.ToEmail(LogEventLevel.Warning, messageTemplate); }

        public void Warning(Exception exception, string messageTemplate)
        { _logger.Warning(exception, messageTemplate); this.ToEmail(LogEventLevel.Warning, messageTemplate, exception); }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        { _logger.Warning(exception, messageTemplate, propertyValues); this.ToEmail(LogEventLevel.Warning, messageTemplate, exception); }

        public void Warning<T>(Exception exception, string messageTemplate, T propertyValue)
        { _logger.Warning(exception, messageTemplate, propertyValue); this.ToEmail(LogEventLevel.Warning, messageTemplate, exception); }

        public void Warning<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        { _logger.Warning(exception, messageTemplate, propertyValue0, propertyValue1); this.ToEmail(LogEventLevel.Warning, messageTemplate, exception); }

        public void Warning<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        { _logger.Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2); this.ToEmail(LogEventLevel.Warning, messageTemplate, exception); }

        #endregion

        #region Error

        public void Error(string messageTemplate)
        { _logger.Error(messageTemplate); this.ToEmail(LogEventLevel.Error, messageTemplate); }

        public void Error(string messageTemplate, params object[] propertyValues)
        { _logger.Error(messageTemplate, propertyValues); this.ToEmail(LogEventLevel.Error, messageTemplate); }

        public void Error<T>(string messageTemplate, T propertyValue)
        { _logger.Error(messageTemplate, propertyValue); this.ToEmail(LogEventLevel.Error, messageTemplate); }

        public void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        { _logger.Error(messageTemplate, propertyValue0, propertyValue1); this.ToEmail(LogEventLevel.Error, messageTemplate); }

        public void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        { _logger.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2); this.ToEmail(LogEventLevel.Error, messageTemplate); }

        public void Error(Exception exception, string messageTemplate)
        { _logger.Error(exception, messageTemplate); this.ToEmail(LogEventLevel.Error, messageTemplate, exception); }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        { _logger.Error(exception, messageTemplate, propertyValues); this.ToEmail(LogEventLevel.Error, messageTemplate, exception); }

        public void Error<T>(Exception exception, string messageTemplate, T propertyValue)
        { _logger.Error(exception, messageTemplate, propertyValue); this.ToEmail(LogEventLevel.Error, messageTemplate, exception); }

        public void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        { _logger.Error(exception, messageTemplate, propertyValue0, propertyValue1); this.ToEmail(LogEventLevel.Error, messageTemplate, exception); }

        public void Error<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        { _logger.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2); this.ToEmail(LogEventLevel.Error, messageTemplate, exception); }

        #endregion

        #region Fatal

        public void Fatal(string messageTemplate)
        { _logger.Fatal(messageTemplate); this.ToEmail(LogEventLevel.Fatal, messageTemplate); }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        { _logger.Fatal(messageTemplate, propertyValues); this.ToEmail(LogEventLevel.Fatal, messageTemplate); }

        public void Fatal<T>(string messageTemplate, T propertyValue)
        { _logger.Fatal(messageTemplate, propertyValue); this.ToEmail(LogEventLevel.Fatal, messageTemplate); }

        public void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        { _logger.Fatal(messageTemplate, propertyValue0, propertyValue1); this.ToEmail(LogEventLevel.Fatal, messageTemplate); }

        public void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        { _logger.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2); this.ToEmail(LogEventLevel.Fatal, messageTemplate); }

        public void Fatal(Exception exception, string messageTemplate)
        { _logger.Fatal(exception, messageTemplate); this.ToEmail(LogEventLevel.Fatal, messageTemplate, exception); }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        { _logger.Fatal(exception, messageTemplate, propertyValues); this.ToEmail(LogEventLevel.Fatal, messageTemplate, exception); }

        public void Fatal<T>(Exception exception, string messageTemplate, T propertyValue)
        { _logger.Fatal(exception, messageTemplate, propertyValue); this.ToEmail(LogEventLevel.Fatal, messageTemplate, exception); }

        public void Fatal<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        { _logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1); this.ToEmail(LogEventLevel.Fatal, messageTemplate, exception); }

        public void Fatal<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        { _logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2); this.ToEmail(LogEventLevel.Fatal, messageTemplate, exception); }

        #endregion

        #region ISeriLogger

        public void Info(string msg)
        { _logger.Write(LogEventLevel.Information, msg); }

        public void Info<T>(T obj)
        { _logger.Write(LogEventLevel.Information, "", obj); }

        public void Error(string msg, Exception exception = null)
        { _logger.Write(LogEventLevel.Error, exception, msg); }

        public void Error<T>(T obj, Exception exception = null)
        { _logger.Write(LogEventLevel.Error, exception, "", obj); }

        #endregion
    }
}
