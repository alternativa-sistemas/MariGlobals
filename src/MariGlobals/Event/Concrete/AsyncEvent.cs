using MariGlobals.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MariGlobals.Events
{
    //Thanks for D#+.
    //https://github.com/DSharpPlus/DSharpPlus/blob/master/DSharpPlus/AsyncEvent.cs

    internal delegate Task NullHandler();

    /// <summary>
    /// A delegate representing an event subscriber.
    /// </summary>
    public delegate Task AsyncEventHandler();

    /// <summary>
    /// A delegate representing an event subscriber.
    /// </summary>
    /// <param name="arg">The object of the current event.</param>
    public delegate Task AsyncEventHandler<T>(T arg);

    /// <summary>
    /// Represents an event that will be invoked asynchronously.
    /// </summary>
    public class AsyncEvent : BaseAsyncEvent
    {
        private List<AsyncEventHandler> Handlers { get; }

        /// <summary>
        /// Creates a new instance of <see cref="AsyncEvent" />.
        /// </summary>
        /// <param name="concurrent">If true all subscribers this event will invokes concurrently, 
        /// default is true.</param>
        /// <param name="waitAll">If false and <param ref="concurrent" /> true this event will 
        /// wait all invokes finishes, default is false.</param>
        public AsyncEvent(bool concurrent = true, bool waitAll = false)
            : base(false, concurrent, waitAll)
        {
            Handlers = new List<AsyncEventHandler>();
        }

        /// <summary>
        /// Subscribe to the current event.
        /// </summary>
        /// <param name="handler">The delegate to be subscribed.</param>
        public void Register(AsyncEventHandler handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Add(handler);
        }

        /// <summary>
        /// Unsubscribe to the current event.
        /// </summary>
        /// <param name="handler">The delegate to be unsubscribed.</param>
        public void Unregister(AsyncEventHandler handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Remove(handler);
        }

        /// <summary>
        /// Asynchronously invokes all subscribers.
        /// </summary>
        public async Task InvokeAsync()
        {
            List<AsyncEventHandler> handlers = null;
            lock (_lock)
                handlers = Handlers;

            if (handlers.HasNoContent())
                return;

            await InvokeAllAsync(handlers).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Represents an event that will be invoked asynchronously.
    /// </summary>
    public class AsyncEvent<T> : BaseAsyncEvent
    {
        private List<AsyncEventHandler<T>> Handlers { get; }

        /// <summary>
        /// Creates a new instance of <see cref="AsyncEvent{T}" />.
        /// </summary>
        /// <param name="concurrent">If true all subscribers this event will invokes concurrently, 
        /// default is true.</param>
        /// <param name="waitAll">If false and <param ref="concurrent" /> true this event will 
        /// wait all invokes finishes, default is false.</param>
        public AsyncEvent(bool concurrent = true, bool waitAll = false)
            : base(true, concurrent, waitAll)
        {
            Handlers = new List<AsyncEventHandler<T>>();
        }

        /// <summary>
        /// Subscribe to the current event.
        /// </summary>
        /// <param name="handler">The delegate to be subscribed.</param>
        public void Register(AsyncEventHandler<T> handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Add(handler);
        }

        /// <summary>
        /// Unsubscribe to the current event.
        /// </summary>
        /// <param name="handler">The delegate to be unsubscribed.</param>
        public void Unregister(AsyncEventHandler<T> handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Remove(handler);
        }

        /// <summary>
        /// Asynchronously invokes all subscribers.
        /// </summary>
        /// <param name="arg">The event object to be passed to all subscribers.</param>
        public async Task InvokeAsync(T arg)
        {
            List<AsyncEventHandler<T>> handlers = null;

            lock (_lock)
                handlers = Handlers;

            if (handlers.HasNoContent())
                return;

            await InvokeAllAsync(handlers, arg).ConfigureAwait(false);
        }
    }
}