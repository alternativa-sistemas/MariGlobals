using System;

namespace MariGlobals.Executor
{
    /// <summary>
    /// Represents a error generated in the <see cref="BaseAsyncQueueExecutor{T}.ActionAsync(T)" /> 
    /// or in the <see cref="BaseQueueExecutor{T}.Action(T)" />.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the queue.</typeparam>
    public class QueueError<T> : EventArgs
    {
        internal QueueError(Exception exception, T queueObject)
        {
            Exception = exception;
            QueueObject = queueObject;
        }

        /// <summary>
        /// The <see cref="Exception" /> ocurried in the execution of this 
        /// <see cref="QueueObject" />.
        /// </summary>
        /// <value></value>
        public Exception Exception { get; }

        /// <summary>
        /// The object of <typeparam ref="T" /> that caused this <see cref="Exception" />.
        /// </summary>
        public T QueueObject { get; }
    }
}