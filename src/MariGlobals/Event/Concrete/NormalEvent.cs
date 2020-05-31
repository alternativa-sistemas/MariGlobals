using MariGlobals.Extensions;
using System;
using System.Collections.Generic;

namespace MariGlobals.Events
{
    /// <summary>
    /// A delegate representing an event subscriber.
    /// </summary>
    public delegate void NormalEventHandler();

    /// <summary>
    /// A delegate representing an event subscriber.
    /// </summary>
    /// <param name="arg">The object of the current event.</param>
    public delegate void NormalEventHandler<T>(T arg);

    /// <summary>
    /// Represents an event that will be invoked synchronously.
    /// </summary>
    public sealed class NormalEvent : BaseNormalEvent
    {
        private List<NormalEventHandler> Handlers { get; }

        /// <summary>
        /// Creates a new instance of <see cref="NormalEvent" />.
        /// </summary>
        public NormalEvent()
        {
            Handlers = new List<NormalEventHandler>();
        }

        /// <summary>
        /// Subscribe to the current event.
        /// </summary>
        /// <param name="handler">The delegate to be subscribed.</param>
        public void Register(NormalEventHandler handler)
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
        public void Unregister(NormalEventHandler handler)
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
            List<NormalEventHandler> handlers = null;
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
    public sealed class NormalEvent<T> : BaseNormalEvent
    {
        private List<NormalEventHandler<T>> Handlers { get; }

        /// <summary>
        /// Creates a new instance of <see cref="NormalEvent{T}" />.
        /// </summary>
        public NormalEvent() : base(true)
        {
            Handlers = new List<NormalEventHandler<T>>();
        }

        /// <summary>
        /// Subscribe to the current event.
        /// </summary>
        /// <param name="handler">The delegate to be subscribed.</param>
        public void Register(NormalEventHandler<T> handler)
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
        public void Unregister(NormalEventHandler<T> handler)
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
            List<NormalEventHandler<T>> handlers = null;

            lock (_lock)
                handlers = Handlers;

            if (handlers.HasNoContent())
                return;

            InvokeAll(handlers, arg);
        }
    }
}