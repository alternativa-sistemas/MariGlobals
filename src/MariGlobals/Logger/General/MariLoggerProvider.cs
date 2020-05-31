using MariGlobals.Logger.Entities;
using MariGlobals.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace MariGlobals.Logger.General
{
    internal readonly struct MariLoggerProvider : ILoggerProvider
    {
        private readonly IConfigurationSection _configuration;
        private readonly MariLoggerConfig _config;
        private readonly ConcurrentDictionary<string, MariLogger> _loggers;
        private readonly MariLoggerSender _sender;
        private readonly MariLoggerWriter _writer;

        public MariLoggerProvider(MariLoggerConfig config, IConfigurationSection configuration)
        {
            _config = config;
            _loggers = new ConcurrentDictionary<string, MariLogger>();
            _configuration = configuration;
            _sender = new MariLoggerSender();
            _writer = new MariLoggerWriter(default);

            _sender.OnLog += OnLog;
        }

        private void OnLog(MariEventLogMessage message)
        {
            if (_config.EnableWriter)
                _writer.WriteLog.Invoke(message);

            _config.SendLog.Invoke(message);
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (_loggers.TryGetValue(categoryName, out var logger))
                return logger;

            var category = _configuration.GetSection(categoryName.Split('.')[0]);
            if (category.Value.HasNoContent())
                category = _configuration.GetSection("Default");

            logger = new MariLogger(categoryName, category, _sender);
            _loggers.TryAdd(categoryName, logger);
            return logger;
        }

        public void Dispose()
        {
            _loggers.Clear();
            _sender.Dispose();
            _writer.Dispose();
        }
    }
}