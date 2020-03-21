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
            Semaphore.WaitAsync();

            Action(obj);

            Semaphore.Release();
            ExecuteNext();
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