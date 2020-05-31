using MariGlobals.Extensions;
using System;
using System.Collections.Generic;

namespace MariGlobals.Events
{
    /// <summary>
    /// A delegate representing an event subscriber.
    /// </summary>
    public delegate void SyncEventHandler();

    /// <summary>
    /// A delegate representing an event subscriber.
    /// </summary>
    /// <param name="arg">The object of the current event.</param>
    public delegate void SyncEventHandler<T>(T arg);

    /// <summary>
    /// Represents an event that will be invoked synchronously.
    /// </summary>
    public class SyncEvent : BaseSyncEvent
    {
        private List<SyncEventHandler> Handlers { get; }

        /// <summary>
        /// Creates a new instance of <see cref="SyncEvent" />.
        /// </summary>
        public SyncEvent()
        {
            Handlers = new List<SyncEventHandler>();
        }

        /// <summary>
        /// Subscribe to the current event.
        /// </summary>
        /// <param name="handler">The delegate to be subscribed.</param>
        public void Register(SyncEventHandler handler)
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
        public void Unregister(SyncEventHandler handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Remove(handler);
        }

        /// <summary>
        /// Synchronously invokes all subscribers.
        /// </summary>
        public void Invoke()
        {
            List<SyncEventHandler> handlers = null;
            lock (_lock)
                handlers = Handlers;

            if (handlers.HasNoContent())
                return;

            InvokeAll(handlers);
        }
    }

    /// <summary>
    /// Represents an event that will be invoked synchronously.
    /// </summary>
    public class SyncEvent<T> : BaseSyncEvent
    {
        private List<SyncEventHandler<T>> Handlers { get; }

        /// <summary>
        /// Creates a new instance of <see cref="SyncEvent{T}" />.
        /// </summary>
        public SyncEvent() : base(true)
        {
            Handlers = new List<SyncEventHandler<T>>();
        }

        /// <summary>
        /// Subscribe to the current event.
        /// </summary>
        /// <param name="handler">The delegate to be subscribed.</param>
        public void Register(SyncEventHandler<T> handler)
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
        public void Unregister(SyncEventHandler<T> handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Remove(handler);
        }

        /// <summary>
        /// Synchronously invokes all subscribers.
        /// </summary>
        /// <param name="arg">The event object to be passed to all subscribers.</param>
        public void Invoke(T arg)
        {
            List<SyncEventHandler<T>> handlers = null;

            lock (_lock)
                handlers = Handlers;

            if (handlers.HasNoContent())
                return;

            InvokeAll(handlers, arg);
        }
    }
}