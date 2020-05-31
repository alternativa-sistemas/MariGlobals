using Microsoft.Extensions.Logging;
using System;

namespace MariGlobals.Logger.Entities
{
    public struct MariLogMessage
    {
        public MariLogMessage(string message, string app, string source, LogLevel logLevel, Exception exception = null)
        {
            Message = message;
            App = app;
            Source = source;
            Exception = exception;
            LogLevel = logLevel;
            Id = Guid.NewGuid().ToString();
            Date = DateTime.Now;
        }

        public MariLogMessage(MariEventLogMessage message, string app)
        {
            App = app;
            Message = message.Message;
            Source = message.SectionName;
            Exception = message.Exception;
            LogLevel = message.LogLevel;
            Id = Guid.NewGuid().ToString();
            Date = DateTime.Now;
        }

        public string Id { get; set; }

        public string Message { get; set; }

        public string App { get; set; }

        public string Source { get; set; }

        public Exception Exception { get; set; }

        public LogLevel LogLevel { get; set; }

        public string FullMessage { get => ToString(); set { FullMessage = value; } }

        public DateTime Date { get; set; }
    }
}