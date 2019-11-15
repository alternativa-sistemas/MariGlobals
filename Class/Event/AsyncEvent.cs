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

    #region Delegates

    internal delegate Task Null();

    public delegate Task AsyncEventHandler();

    public delegate Task AsyncEventHandler<T>(T arg);

    #endregion Delegates

    #region BaseAsyncEvent

    public class BaseAsyncEvent
    {
        public Task InvokeAllAsync<T>(List<T> handlers)
            => InvokeAllAsync<T, Null>(handlers);

        public async Task InvokeAllAsync<T, T2>(List<T> handlers, T2 arg = default)
        {
            var exceptions = new List<Exception>(handlers.Count);

            foreach (var handler in handlers)
            {
                try
                {
                    if (handlers.FirstOrDefault().IsGeneric())
                    {
                        await ExplicitConvert<T, T2>
                                (handler, typeof(AsyncEventHandler<>)
                                        .MakeGenericType(handler.GetFirstGenericType()))(arg)
                                            .ConfigureAwait(false);
                    }
                    else if (handlers.OfType<AsyncEventHandler>().Any())
                    {
                        await (handler as AsyncEventHandler)().ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
                throw new AggregateException("Exceptions occured within one or more event handlers. Check InnerExceptions for details.", exceptions);
        }

        private AsyncEventHandler<TResult> ExplicitConvert<TSource, TResult>(TSource source, Type type)
            => (AsyncEventHandler<TResult>)Convert.ChangeType(source, type);
    }

    #endregion BaseAsyncEvent

    #region NormalAsyncEvent

    public sealed class AsyncEvent : BaseAsyncEvent
    {
        private readonly object _lock = new object();
        private List<AsyncEventHandler> Handlers { get; }

        public AsyncEvent()
        {
            Handlers = new List<AsyncEventHandler>();
        }

        #region Register

        public void Register(AsyncEventHandler handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Add(handler);
        }

        #endregion Register

        #region Unregister

        public void Unregister(AsyncEventHandler handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Remove(handler);
        }

        #endregion Unregister

        #region InvokeAsync

        public async Task InvokeAsync()
        {
            List<AsyncEventHandler> handlers = null;
            lock (_lock)
                handlers = Handlers;

            if (handlers.HasNoContent())
                return;

            await InvokeAllAsync(handlers).ConfigureAwait(false);
        }

        #endregion InvokeAsync
    }

    #endregion NormalAsyncEvent

    #region GenericAsyncEvent

    public sealed class AsyncEvent<T> : BaseAsyncEvent
    {
        private readonly object _lock = new object();
        private List<AsyncEventHandler<T>> Handlers { get; }

        public AsyncEvent()
        {
            Handlers = new List<AsyncEventHandler<T>>();
        }

        #region Register

        public void Register(AsyncEventHandler<T> handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Add(handler);
        }

        #endregion Register

        #region Unregister

        public void Unregister(AsyncEventHandler<T> handler)
        {
            if (handler.HasNoContent())
                throw new ArgumentNullException(nameof(handler), "Handler cannot be null");

            lock (_lock)
                Handlers.Remove(handler);
        }

        #endregion Unregister

        #region InvokeAsync

        public async Task InvokeAsync(T arg)
        {
            List<AsyncEventHandler<T>> handlers = null;

            lock (_lock)
                handlers = Handlers;

            if (handlers.HasNoContent())
                return;

            await InvokeAllAsync(handlers, arg).ConfigureAwait(false);
        }

        #endregion InvokeAsync
    }

    #endregion GenericAsyncEvent
}