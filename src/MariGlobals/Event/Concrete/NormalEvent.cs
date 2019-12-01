using MariGlobals.Event.Base;
using MariGlobals.Utils;
using System;
using System.Collections.Generic;

namespace MariGlobals.Event.Concrete
{
    public delegate void NormalEventHandler();

    public delegate void NormalEventHandler<T>(T arg);

    public sealed class NormalEvent : BaseNormalEvent
    {
        private List<NormalEventHandler> Handlers { get; }

        public NormalEvent()
        {
            Handlers = new List<NormalEventHandler>();
        }

        public void Register(NormalEventHandler handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Add(handler);
        }

        public void Unregister(NormalEventHandler handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Remove(handler);
        }

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

    public sealed class NormalEvent<T> : BaseNormalEvent
    {
        private List<NormalEventHandler<T>> Handlers { get; }

        public NormalEvent() : base(true)
        {
            Handlers = new List<NormalEventHandler<T>>();
        }

        public void Register(NormalEventHandler<T> handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Add(handler);
        }

        public void Unregister(NormalEventHandler<T> handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Remove(handler);
        }

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