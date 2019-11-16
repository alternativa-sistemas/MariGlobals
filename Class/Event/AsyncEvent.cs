using MariGlobals.Class.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariGlobals.Class.Event
{
    //Thanks for D#+.
    //https://github.com/DSharpPlus/DSharpPlus/blob/master/DSharpPlus/AsyncEvent.cs

    internal delegate Task NullHandler();

    public delegate Task AsyncEventHandler();

    public delegate Task AsyncEventHandler<T>(T arg);

    public sealed class AsyncEvent : BaseAsyncEvent
    {
        private List<AsyncEventHandler> Handlers { get; }

        public AsyncEvent()
        {
            Handlers = new List<AsyncEventHandler>();
        }

        public void Register(AsyncEventHandler handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Add(handler);
        }

        public void Unregister(AsyncEventHandler handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Remove(handler);
        }

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

    public sealed class AsyncEvent<T> : BaseAsyncEvent
    {
        private List<AsyncEventHandler<T>> Handlers { get; }

        public AsyncEvent() : base(true)
        {
            Handlers = new List<AsyncEventHandler<T>>();
        }

        public void Register(AsyncEventHandler<T> handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Add(handler);
        }

        public void Unregister(AsyncEventHandler<T> handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Remove(handler);
        }

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