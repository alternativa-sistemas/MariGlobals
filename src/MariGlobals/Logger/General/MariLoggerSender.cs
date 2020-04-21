using MariGlobals.Event.Concrete;
using MariGlobals.Logger.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MariGlobals.Logger.General
{
    internal struct MariLoggerSender : IDisposable
    {
        private readonly SemaphoreSlim Semaphore;

        private readonly ConcurrentQueue<MariEventLogMessage> LogsQueue;

        public MariLoggerSender(int _)
        {
            Semaphore = new SemaphoreSlim(1, 1);
            LogsQueue = new ConcurrentQueue<MariEventLogMessage>();
            SendLog = new NormalEvent<MariEventLogMessage>();
            OnLogSender = new NormalEvent<MariEventLogMessage>();

            IsDisposed = false;

            OnLogReceived += LogReceived;
        }

        private event NormalEventHandler<MariEventLogMessage> OnLogReceived
        {
            add => SendLog.Register(value);
            remove => SendLog.Unregister(value);
        }

        public readonly NormalEvent<MariEventLogMessage> SendLog;

        public event NormalEventHandler<MariEventLogMessage> OnLog
        {
            add => OnLogSender.Register(value);
            remove => OnLogSender.Unregister(value);
        }

        private readonly NormalEvent<MariEventLogMessage> OnLogSender;

        public bool IsDisposed { get; private set; }

        private bool CanCreateThread
            => Semaphore.CurrentCount > 0;

        private void LogReceived(MariEventLogMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.Message) || string.IsNullOrWhiteSpace(message.SectionName))
                return;

            LogsQueue.Enqueue(message);
            TryCreateNewThread();
        }

        private void WriteLog(MariEventLogMessage message)
        {
            Semaphore.Wait();

            OnLogSender.Invoke(message);

            Semaphore.Release();
            WriteNextLog();
        }

        private void WriteNextLog()
        {
            if (LogsQueue.Count > 0 && LogsQueue.TryDequeue(out var message))
            {
                WriteLog(message);
            }
        }

        private void TryCreateNewThread()
        {
            if (CanCreateThread)
                _ = Task.Run(WriteNextLog);
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            Semaphore.Dispose();
        }
    }
}