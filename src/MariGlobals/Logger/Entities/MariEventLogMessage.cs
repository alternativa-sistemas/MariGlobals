using Microsoft.Extensions.Logging;
using System;

namespace MariGlobals.Logger
{
    /// <summary>
    /// Represents a generic MariGlobals.Logger log message.
    /// </summary>
    public readonly struct MariEventLogMessage
    {
        internal MariEventLogMessage(string message, string sectionName, LogLevel logLevel, Exception exception)
        {
            Message = message;
            SectionName = sectionName;
            LogLevel = logLevel;
            Exception = exception;
        }

        /// <summary>
        /// The message of this log.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// The section where this log comes from.
        /// </summary>
        public string SectionName { get; }

        /// <summary>
        /// The <see cref="LogLevel" /> of this log.
        /// </summary>
        public LogLevel LogLevel { get; }

        /// <summary>
        /// The <see cref="Exception" /> of this log.
        /// Can be null.
        /// </summary>
        public Exception Exception { get; }
    }
}