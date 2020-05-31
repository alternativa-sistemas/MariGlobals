using System;

namespace MariGlobals.Executor
{
    public class QueueError<T> : EventArgs
    {
        internal QueueError(Exception exception, T queueObject)
        {
            Exception = exception;
            QueueObject = queueObject;
        }

        public Exception Exception { get; }

        public T QueueObject { get; }
    }
}