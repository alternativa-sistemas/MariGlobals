using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using MariGlobals.Event.Concrete;

namespace MariGlobals.Executor
{
    public abstract class BaseQueueExecutor<T>
    {
        public BaseQueueExecutor(int maxThreads = 1)
        {
            Semaphore = new SemaphoreSlim(1, maxThreads);
            _onError = new NormalEvent<QueueError<T>>();
            Queue = new ConcurrentQueue<T>();
            SendObj = new NormalEvent<T>();
            IsDisposed = false;

            OnObjReceived += ObjReceived;
        }

        private event NormalEventHandler<T> OnObjReceived
        {
            add => SendObj.Register(value);
            remove => SendObj.Unregister(value);
        }

        protected readonly NormalEvent<T> SendObj;

        protected event NormalEventHandler<QueueError<T>> OnError
        {
            add => _onError.Register(value);
            remove => _onError.Unregister(value);
        }

        private readonly NormalEvent<QueueError<T>> _onError;

        private readonly SemaphoreSlim Semaphore;

        private readonly ConcurrentQueue<T> Queue;

        public bool IsDisposed { get; private set; }

        private bool CanCreateThread
            => Semaphore.CurrentCount > 0;

        protected abstract void Action(T obj);

        private void ObjReceived(T obj)
        {
            Queue.Enqueue(obj);
            TryCreateNewThread();
        }

        private void Execute(T obj)
        {
            try
            {
                Semaphore.WaitAsync();
                Action(obj);
            }
            catch (Exception ex)
            {
                _onError.Invoke(new QueueError<T>(ex, obj));
            }
            finally
            {
                Semaphore.Release();
                ExecuteNext();
            }
        }

        private void ExecuteNext()
        {
            if (Queue.Count > 0 && Queue.TryDequeue(out var obj))
                Execute(obj);
        }

        private void TryCreateNewThread()
        {
            if (CanCreateThread)
            {
                _ = Task.Run(ExecuteNext);
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