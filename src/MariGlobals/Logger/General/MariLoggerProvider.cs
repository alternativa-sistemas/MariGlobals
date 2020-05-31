using MariGlobals.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text;

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
            _configuration = configuration;

            _loggers = new ConcurrentDictionary<string, MariLogger>();
            _sender = new MariLoggerSender();
            _writer = new MariLoggerWriter();

            _sender.OnLog += OnLog;
        }

        private void OnLog(MariEventLogMessage message)
        {
            if (_config.EnableWriter)
                _writer.AddLog(message);

            _config.SendLog.Invoke(message);
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (_loggers.TryGetValue(categoryName, out var logger))
                return logger;

            var category = GetSection(categoryName);

            logger = new MariLogger(categoryName, category, _sender);

            _loggers.TryAdd(categoryName, logger);

            return logger;
        }

        private IConfigurationSection GetSection(string categoryName)
        {
            var defaultCategory = _configuration.GetSection("Default");

            if (string.IsNullOrWhiteSpace(categoryName))
                return defaultCategory;

            var names = categoryName.Split('.');
            var builder = new StringBuilder();

            var category = defaultCategory;

            for (var i = 0; i < names.Length; i++)
            {
                var name = names[i];

                if (i == 0)
                    builder.Append(name);
                else
                    builder.Append($".{name}");

                var section = _configuration.GetSection(builder.ToString());

                if (section.HasContent())
                    category = section;
            }

            return category;
        }

        public void Dispose()
        {
            _loggers.Clear();
            _sender.Dispose();
            _writer.Dispose();
        }
    }
}