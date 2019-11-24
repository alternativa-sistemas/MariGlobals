using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MariGlobals.Structs.Logger
{
    public readonly struct MariEventLogMessage
    {
        internal MariEventLogMessage(string message, string sectionName, LogLevel logLevel, Exception exception)
        {
            Message = message;
            SectionName = sectionName;
            LogLevel = logLevel;
            Exception = exception;
        }

        public string Message { get; }

        public string SectionName { get; }

        public LogLevel LogLevel { get; }

        public Exception Exception { get; }
    }
}