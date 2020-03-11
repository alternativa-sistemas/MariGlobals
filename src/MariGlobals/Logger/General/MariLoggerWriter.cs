﻿using Colorful;
using MariGlobals.Event.Concrete;
using MariGlobals.Logger.Entities;
using MariGlobals.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace MariGlobals.Logger.General
{
    public struct MariLoggerWriter : IDisposable
    {
        internal MariLoggerWriter(int _)
        {
            Semaphore = new SemaphoreSlim(1, 1);
            LogsQueue = new Queue<MariEventLogMessage>();
            WriteLog = new NormalEvent<MariEventLogMessage>();
            IsDisposed = false;
            OnLog += LogReceived;
        }

        private event NormalEventHandler<MariEventLogMessage> OnLog
        {
            add => WriteLog.Register(value);
            remove => WriteLog.Unregister(value);
        }

        public readonly NormalEvent<MariEventLogMessage> WriteLog;

        private readonly SemaphoreSlim Semaphore;

        private readonly Queue<MariEventLogMessage> LogsQueue;

        public bool IsDisposed { get; private set; }

        private bool CanCreateThread
            => Semaphore.CurrentCount > 0;

        private void LogReceived(MariEventLogMessage message)
        {
            LogsQueue.Enqueue(message);
            TryCreateNewThread();
        }

        private async ValueTask WriteLogAsync(MariEventLogMessage logQueueMessage)
        {
            await Semaphore.WaitAsync();
            var date = DateTimeOffset.Now;
            var (color, abbreviation) = logQueueMessage.LogLevel.LogLevelInfo();

            const string logMessage = "[{0}] [{1}] [{2}]\n    {3}";
            var formatters = new[]
            {
                new Formatter($"{date:MMM d - hh:mm:ss tt}", Color.Gray),
                new Formatter(abbreviation, color),
                new Formatter(logQueueMessage.SectionName, color),
                new Formatter(logQueueMessage.Message, Color.Wheat)
            };

            Console.WriteLineFormatted(logMessage, Color.White, formatters);

            Semaphore.Release();
            await WriteNextLogAsync();
        }

        private async ValueTask WriteNextLogAsync()
        {
            if (LogsQueue.Count > 0)
            {
                await WriteLogAsync(LogsQueue.Dequeue());
            }
        }

        private void TryCreateNewThread()
        {
            if (CanCreateThread)
            {
                var instance = this;
                _ = Task.Run(WriteNextLogAsync);
            }
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            Semaphore.Dispose();
        }
    }
}