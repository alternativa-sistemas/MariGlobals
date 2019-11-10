using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MariGlobals.Class.Logger
{
    public class MariLoggerConfig
    {
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Information;

        public int EventId { get; set; } = 0;

        private readonly object _lock = new object();

        public event Action<LogLevel, Exception, string, string> OnLog
        {
            add
            {
                lock (_lock)
                {
                    _log += value;
                }
            }
            remove
            {
                lock (_lock)
                {
                    _log -= value;
                }
            }
        }

        internal Action<LogLevel, Exception, string, string> _log;
    }
}