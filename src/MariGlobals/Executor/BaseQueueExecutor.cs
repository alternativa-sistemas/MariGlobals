using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using MariGlobals.Events;

namespace MariGlobals.Executor
{
    /// <summary>
    /// A concurent action executor for object of <typeparam ref="T" />
    /// </summary>
    /// <typeparam name="T">The type of the objects will be queued.</typeparam>
    public abstract class BaseQueueExecutor<T>
    {
        /// <summary>
        /// Creates a new instance of <see cref="BaseQueueExecutor{T}" />.
        /// </summary>
        /// <param name="maxThreads">The max number of threads to be used for this executor.</param>
        public BaseQueueExecutor(int maxThreads = 1)
        {
            Semaphore = new SemaphoreSlim(1, maxThreads);
            Queue = new ConcurrentQueue<T>();
            IsDisposed = false;
        }

        /// <summary>
        /// Fired when a error is generated in the execution of an action.
        /// </summary>
        protected event NormalEventHandler<QueueError<T>> OnError
        {
            add => _onError.Register(value);
            remove => _onError.Unregister(value);
        }

        private readonly NormalEvent<QueueError<T>> _onError = new NormalEvent<QueueError<T>>();

        private readonly SemaphoreSlim Semaphore;

        private readonly ConcurrentQueue<T> Queue;

        /// <summary>
        /// Wheter this executor is disposed or not.
        /// </summary>
        public bool IsDisposed { get; private set; }

        private bool CanCreateThread
            => Semaphore.CurrentCount > 0;

        /// <summary>
        /// The action to be executed concurrency per <param ref="obj" />.
        /// </summary>
        /// <param name="obj">A object in the queue to be executed.</param>
        protected abstract void Action(T obj);

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
            if (!IsDisposed && Queue.Count > 0 && Queue.TryDequeue(out var obj))
                Execute(obj);
        }

        private void TryCreateNewThread()
        {
            if (CanCreateThread)
            {
                _ = Task.Run(ExecuteNext);
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