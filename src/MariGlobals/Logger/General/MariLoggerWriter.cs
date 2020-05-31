using Colorful;
using MariGlobals.Executor;
using MariGlobals.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using Console = Colorful.Console;

namespace MariGlobals.Logger.General
{
    internal class MariLoggerWriter : BaseQueueExecutor<MariEventLogMessage>
    {
        public MariLoggerWriter()
        {
        }

        public void AddLog(MariEventLogMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.Message) || string.IsNullOrWhiteSpace(message.SectionName))
                return;

            AddObject(message);
        }

        protected override void Action(MariEventLogMessage logQueueMessage)
        {
            var date = DateTimeOffset.Now;
            var (color, abbreviation) = logQueueMessage.LogLevel.LogLevelInfo();

            var logMessage = "[{0}] [{1}] [{2}]\n    {3}";
            var formatters = new List<Formatter>
            {
                new Formatter($"{date:MMM d - hh:mm:ss tt}", Color.Gray),
                new Formatter(abbreviation, color),
                new Formatter(logQueueMessage.SectionName, color),
                new Formatter(logQueueMessage.Message, Color.Wheat)
            };

            if (logQueueMessage.Exception.HasContent())
            {
                logMessage += "\n    {4}";
                formatters.Add(new Formatter(logQueueMessage.Exception.ToString(), Color.Wheat));
            }

            Console.WriteLineFormatted(logMessage, Color.White, formatters.ToArray());
        }
    }
}