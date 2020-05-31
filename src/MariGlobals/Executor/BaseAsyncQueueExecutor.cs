using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using MariGlobals.Events;

namespace MariGlobals.Executor
{
    /// <summary>
    /// A concurent async action executor for object of <typeparam ref="T" />
    /// </summary>
    /// <typeparam name="T">The type of the objects will be queued.</typeparam>
    public abstract class BaseAsyncQueueExecutor<T>
    {
        /// <summary>
        /// Creates a new instance of <see cref="BaseAsyncQueueExecutor{T}" />.
        /// </summary>
        /// <param name="maxThreads">The max number of threads to be used for this executor.</param>
        public BaseAsyncQueueExecutor(int maxThreads = 1)
        {
            Semaphore = new SemaphoreSlim(1, maxThreads);
            Queue = new ConcurrentQueue<T>();
            IsDisposed = false;
        }

        /// <summary>
        /// Fired when a error is generated in the execution of an action.
        /// </summary>
        protected event AsyncEventHandler<QueueError<T>> OnError
        {
            add => _onError.Register(value);
            remove => _onError.Unregister(value);
        }

        private readonly AsyncEvent<QueueError<T>> _onError = new AsyncEvent<QueueError<T>>();

        private readonly SemaphoreSlim Semaphore;

        private readonly ConcurrentQueue<T> Queue;

        /// <summary>
        /// Wheter this executor is disposed or not.
        /// </summary>
        public bool IsDisposed { get; private set; }

        private bool CanCreateThread
            => Semaphore.CurrentCount > 0;

        /// <summary>
        /// The asynchronous action to be executed concurrency per <param ref="obj" />.
        /// </summary>
        /// <param name="obj">A object in the queue to be executed.</param>
        protected abstract Task ActionAsync(T obj);

        /// <summary>
        /// Add a object of <typeparam ref="T" /> to the queue.
        /// </summary>
        /// <param name="obj">The object to be added in the queue.</param>
        protected void AddObject(T obj)
        {
            if (IsDisposed)
                return;

            Queue.Enqueue(obj);
            TryCreateNewThread();
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
            if (!IsDisposed && Queue.Count > 0 && Queue.TryDequeue(out var obj))
                await ExecuteAsync(obj);
        }

        private void TryCreateNewThread()
        {
            if (CanCreateThread)
            {
                _ = Task.Run(ExecuteNextAsync);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (IsDisposed)
                return;

            Queue.Clear();
            Semaphore.Dispose();
        }
    }
}