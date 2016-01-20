using System;
using System.Collections.Generic;
using NLog;

namespace LFGenerator2.Trace
{
    public interface ILogger
    {
        string Name { get; }
        void Log(LoggerEntryLevel level, string message, Exception exception = null);
    }

    public class Logger : ILogger
    {
        private readonly Dictionary<string, object> _staticProperties = new Dictionary<string, object>();
        private readonly NLog.Logger _log;

        public Logger(string loggerName)
        {
            Name = loggerName;
            _log = LogManager.GetLogger(loggerName);
        }

        public string Name { get; private set; }

        public void Log(LoggerEntryLevel level, string message, Exception exception = null)
        {
            var logEvent = new LogEventInfo(LogLevel.FromOrdinal((int)level), Name, message) { Exception = exception };

            foreach (string key in _staticProperties.Keys)
                logEvent.Properties[key] = _staticProperties[key];

            _log.Log(logEvent);
        }
    }
}
