using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MariGlobals.Events.GenericHandlers;
using MariGlobals.Extensions;

#pragma warning disable IDE0060 // Remove unused parameter

namespace MariGlobals.Events
{
    /// <summary>
    /// Base class for async events.
    /// </summary>
    public class BaseAsyncEvent
    {
        private readonly bool IsGeneric;
        private readonly bool InvokeConcurrent;
        private readonly bool WaitAll;

        /// <summary>
        /// An <see cref="object" /> to use for lock threads in this event.
        /// </summary>
        protected readonly object _lock = new object();

        internal BaseAsyncEvent(bool isGeneric = false, bool concurrent = true, bool waitAll = false)
        {
            IsGeneric = isGeneric;
            InvokeConcurrent = concurrent;
            WaitAll = waitAll;
        }

        /// <summary>
        /// Asynchronously invokes all subscribers.
        /// </summary>
        /// <param name="handlers">All subscribers.</param>
        protected Task InvokeAllAsync<T>(List<T> handlers)
            => InvokeAllAsync<T, NullHandler>(handlers);

        /// <summary>
        /// Asynchronously invokes all subscribers.
        /// </summary>
        /// <param name="handlers">All subscribers.</param>
        /// <param name="arg">The event object to be passed to all subscribers. </param>
        protected async Task InvokeAllAsync<T, T2>(List<T> handlers, T2 arg = default)
        {
            var exceptions = MariMemoryExtensions.CreateMemory<Exception>(handlers.Count);
            var tasks = MariMemoryExtensions.CreateMemory<Task>(handlers.Count);

            foreach (var handler in ConvertList<T, T2>(handlers, arg).ToList())
            {
                if (InvokeConcurrent)
                {
                    tasks.TryAdd(Task.Run(() => InvokeHandlerAsync(handler, arg, exceptions)));
                }
                else
                {
                    await InvokeHandlerAsync(handler, arg, exceptions).ConfigureAwait(false);
                }
            }

            if (InvokeConcurrent && WaitAll)
            {
                var allTasks = Task.WhenAll(tasks.ToArray());

                await allTasks.ConfigureAwait(false);
            }

            if (exceptions.HasContent())
                throw new AggregateException(
                    "Exceptions occured within one or more event handlers. " +
                    "Check InnerExceptions for details.", exceptions.ToArray());
        }

        private async Task InvokeHandlerAsync<T>(
            GenericAsyncEventHandler<T> handler,
            T arg,
            Memory<Exception> exceptions)
        {
            try
            {
                await handler.InvokeAsync(arg).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (InvokeConcurrent)
                    throw ex;

                exceptions.TryAdd(ex);
            }
        }

        private List<GenericAsyncEventHandler<T2>> ConvertList<T1, T2>(List<T1> handlers, T2 arg)
        {
            if (!IsGeneric)
                return ConvertToNormal(handlers) as List<GenericAsyncEventHandler<T2>>;
            else
                return ConvertToGeneric<T1, T2>(handlers);
        }

        private List<GenericAsyncEventHandler<NullHandler>> ConvertToNormal<T>(List<T> handlers)
            => handlers
                .Select(a => new GenericAsyncEventHandler<NullHandler>(a as AsyncEventHandler))
                .ToList();

        private List<GenericAsyncEventHandler<T2>> ConvertToGeneric<T1, T2>(List<T1> handlers)
            => handlers
                .Select(a => new GenericAsyncEventHandler<T2>(a as AsyncEventHandler<T2>))
                .ToList();
    }
}