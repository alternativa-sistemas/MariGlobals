using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MariGlobals.Class.Logger
{
    public class MariLogger : ILogger
    {
        private readonly MariLoggerConfig _config;
        private readonly string Name;

        public MariLogger(MariLoggerConfig loggerConfig, string name)
        {
            _config = loggerConfig;
            Name = name;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _config.MinimumLogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            if (_config.EventId == 0 || _config.EventId == eventId.Id)
                _ = Task.Run(()
                    => _config._log?.Invoke(logLevel, exception, formatter(state, exception), Name));
        }
    }
}