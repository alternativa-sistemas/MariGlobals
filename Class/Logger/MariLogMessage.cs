using Discord;
using MariGlobals.Enum.Logger;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MariGlobals.Class.Logger
{
    public class MariLogMessage
    {
        public MariLogMessage(string message, string app, string source, Exception exception, MariLogSeverity severity)
        {
            Message = message;
            App = app;
            Source = source;
            Exception = exception;
            Severity = severity;
        }

        public MariLogMessage(string app, LogLevel level, Exception ex, string formatedMessage, string source)
        {
            Message = formatedMessage;
            App = app;
            Source = source;
            Exception = ex;
            Severity = ConvertLogLevel(level);
        }

        public MariLogMessage(LogMessage logMessage, string app)
        {
            App = app;
            Message = logMessage.Message;
            Source = logMessage.Source;
            Exception = logMessage.Exception;
            Severity = ConvertSeverity(logMessage.Severity);
        }

        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Message { get; set; }

        public string App { get; set; }

        public string Source { get; set; }

        public Exception Exception { get; set; }

        [BsonRepresentation(BsonType.String)]
        public MariLogSeverity Severity { get; set; }

        public string FullMessage { get => ToString(); set { FullMessage = value; } }

        public DateTime Date { get; set; } = DateTime.Now;

        private MariLogSeverity ConvertLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                default:
                    throw new InvalidOperationException($"Invalid {nameof(LogLevel)} type: {logLevel}");

                case LogLevel.Critical:
                    return MariLogSeverity.Critical;

                case LogLevel.Debug:
                    return MariLogSeverity.Verbose;

                case LogLevel.Error:
                    return MariLogSeverity.Error;

                case LogLevel.None:
                    return MariLogSeverity.Debug;

                case LogLevel.Information:
                    return MariLogSeverity.Info;

                case LogLevel.Trace:
                    return MariLogSeverity.Debug;

                case LogLevel.Warning:
                    return MariLogSeverity.Warning;
            }
        }

        private MariLogSeverity ConvertSeverity(LogSeverity severity)
        {
            switch (severity)
            {
                default:
                    throw new InvalidOperationException($"Invalid {nameof(LogSeverity)} type: {severity}");

                case LogSeverity.Critical:
                    return MariLogSeverity.Critical;

                case LogSeverity.Debug:
                    return MariLogSeverity.Debug;

                case LogSeverity.Error:
                    return MariLogSeverity.Error;

                case LogSeverity.Info:
                    return MariLogSeverity.Info;

                case LogSeverity.Verbose:
                    return MariLogSeverity.Verbose;

                case LogSeverity.Warning:
                    return MariLogSeverity.Warning;
            }
        }

        public override string ToString()
        {
            //Thanks to Discord.Net ToString() System.

            var padSource = 11;
            var sourceName = Source;
            var ex = Exception;

            int maxLength = 1 + 8 + 1 +
                padSource + 1 + (string.IsNullOrWhiteSpace(Message) ? 0 : Message.Length) +
                (string.IsNullOrWhiteSpace(ex?.ToString()) ? 0 : ex.ToString().Length) + 3;

            var builder = new StringBuilder(maxLength);

            builder.Append($"{DateTime.Now.ToString()} ");

            if (sourceName.Length < padSource)
            {
                builder.Append(sourceName);
                builder.Append(' ', padSource - sourceName.Length);
            }
            else if (sourceName.Length > padSource)
                builder.Append(sourceName.Substring(0, padSource));
            else
                builder.Append(sourceName);

            builder.Append(' ');

            if (!string.IsNullOrWhiteSpace(Message))
            {
                for (int i = 0; i < Message.Length; i++)
                {
                    char c = Message[i];
                    if (!char.IsControl(c))
                        builder.Append(c);
                }
            }
            if (!string.IsNullOrWhiteSpace(ex?.ToString()))
            {
                if (!string.IsNullOrWhiteSpace(ex?.ToString()))
                {
                    builder.Append(':');
                    builder.AppendLine();
                }
                builder.Append(ex?.ToString());
            }

            return builder.ToString();
        }
    }
}