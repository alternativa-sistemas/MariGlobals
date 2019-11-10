using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MariGlobals.Class.Logger
{
    public class MariLoggerProvider : ILoggerProvider
    {
        private readonly MariLoggerConfig _config;
        private readonly ConcurrentDictionary<string, MariLogger> _loggers = new ConcurrentDictionary<string, MariLogger>();

        public MariLoggerProvider(MariLoggerConfig config)
        {
            _config = config;
        }

        public ILogger CreateLogger(string categoryName)
            => _loggers.GetOrAdd(categoryName, name => new MariLogger(_config, name));

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}