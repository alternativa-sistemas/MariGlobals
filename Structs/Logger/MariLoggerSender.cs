using MariGlobals.Class.Event;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MariGlobals.Structs.Logger
{
    internal struct MariLoggerSender : IDisposable
    {
        private readonly SemaphoreSlim Semaphore;

        private readonly Queue<MariEventLogMessage> LogsQueue;

        public MariLoggerSender(int _)
        {
            Semaphore = new SemaphoreSlim(1, 1);
            LogsQueue = new Queue<MariEventLogMessage>();
            SendLog = new NormalEvent<MariEventLogMessage>();
            OnLogSender = new NormalEvent<MariEventLogMessage>();

            QueueStopped = true;
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

        private bool QueueStopped { get; set; }

        public bool IsDisposed { get; private set; }

        private bool CanCreateThread
            => QueueStopped && Semaphore.CurrentCount > 0;

        private void LogReceived(MariEventLogMessage message)
        {
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
            if (LogsQueue.Count <= 0)
            {
                QueueStopped = true;
            }
            else
            {
                QueueStopped = false;
                WriteLog(LogsQueue.Dequeue());
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