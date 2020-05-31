using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using MariGlobals.Events;

namespace MariGlobals.Executor
{
    public abstract class BaseAsyncQueueExecutor<T>
    {
        public BaseAsyncQueueExecutor(int maxThreads = 1)
        {
            Semaphore = new SemaphoreSlim(1, maxThreads);
            Queue = new ConcurrentQueue<T>();
            IsDisposed = false;
        }

        protected event AsyncEventHandler<QueueError<T>> OnError
        {
            add => _onError.Register(value);
            remove => _onError.Unregister(value);
        }

        private readonly AsyncEvent<QueueError<T>> _onError = new AsyncEvent<QueueError<T>>();

        private readonly SemaphoreSlim Semaphore;

        private readonly ConcurrentQueue<T> Queue;

        public bool IsDisposed { get; private set; }

        private bool CanCreateThread
            => Semaphore.CurrentCount > 0;

        protected abstract Task ActionAsync(T obj);

        protected Task AddObject(T obj)
        {
            Queue.Enqueue(obj);
            TryCreateNewThread();

            return Task.CompletedTask;
        }

        private async Task ExecuteAsync(T obj)
        {
            try
            {
                await Semaphore.WaitAsync();

                await ActionAsync(obj);
            }
            catch (Exception ex)
            {
                await _onError.InvokeAsync(new QueueError<T>(ex, obj));
            }
            finally
            {
                Semaphore.Release();
                await ExecuteNextAsync();
            }
        }

        private async Task ExecuteNextAsync()
        {
            if (Queue.Count > 0 && Queue.TryDequeue(out var obj))
                await ExecuteAsync(obj);
        }

        private void TryCreateNewThread()
        {
            if (CanCreateThread)
            {
                _ = Task.Run(ExecuteNextAsync);
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