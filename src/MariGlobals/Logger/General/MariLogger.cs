using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace MariGlobals.Logger.General
{
    internal readonly struct MariLogger : ILogger
    {
        private readonly IConfigurationSection _configuration;
        private readonly string Name;
        private readonly MariLoggerSender _sender;

        public MariLogger(string name, IConfigurationSection configuration, MariLoggerSender sender)
        {
            _configuration = configuration;
            Name = name;
            _sender = sender;
        }

        public IDisposable BeginScope<TState>(TState state)
            => default;

        public bool IsEnabled(LogLevel logLevel)
            => (LogLevel)Enum.Parse(typeof(LogLevel), _configuration.Value) <= logLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            try
            {
                var message = formatter(state, exception);
                if (string.IsNullOrWhiteSpace(message))
                    return;

                _sender.AddLog(new MariEventLogMessage(message, Name, logLevel, exception));
            }
            catch { }
        }
    }
}