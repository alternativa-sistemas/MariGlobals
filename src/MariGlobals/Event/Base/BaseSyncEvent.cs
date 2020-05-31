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
    /// Base class for sync events.
    /// </summary>
    public class BaseSyncEvent
    {
        private readonly bool IsGeneric;
        private readonly bool InvokeConcurrent;

        /// <summary>
        /// An <see cref="object" /> to use for lock threads in this event.
        /// </summary>
        protected readonly object _lock = new object();

        internal BaseSyncEvent(bool isGeneric = false, bool concurrent = true)
        {
            IsGeneric = isGeneric;
            InvokeConcurrent = concurrent;
        }

        /// <summary>
        /// Synchronously invokes all subscribers.
        /// </summary>
        /// <param name="handlers">All subscribers.</param>
        protected void InvokeAll<T>(List<T> handlers)
            => InvokeAll<T, NullHandler>(handlers);

        /// <summary>
        /// Synchronously invokes all subscribers.
        /// </summary>
        /// <param name="handlers">All subscribers.</param>
        /// <param name="arg">The event object to be passed to all subscribers. </param>
        protected void InvokeAll<T, T2>(List<T> handlers, T2 arg = default)
        {
            var exceptions = MariMemoryExtensions.CreateMemory<Exception>(handlers.Count);

            foreach (var handler in ConvertList<T, T2>(handlers, arg).ToList())
            {
                if (InvokeConcurrent)
                {
                    _ = Task.Run(() => InvokeHandler(handler, arg, exceptions));
                }
                else
                {
                    InvokeHandler(handler, arg, exceptions);
                }
            }

            if (exceptions.HasContent())
                throw new AggregateException(
                    "Exceptions occured within one or more event handlers. " +
                    "Check InnerExceptions for details.", exceptions.ToArray());
        }

        private void InvokeHandler<T>(
            GenericSyncEventHandler<T> handler,
            T arg,
            Memory<Exception> exceptions)
        {
            try
            {
                handler.Invoke(arg);
            }
            catch (Exception ex)
            {
                if (InvokeConcurrent)
                    throw ex;

                exceptions.TryAdd(ex);
            }
        }

        private List<GenericSyncEventHandler<T2>> ConvertList<T1, T2>(List<T1> handlers, T2 arg)
        {
            if (!IsGeneric)
                return ConvertToNormal(handlers) as List<GenericSyncEventHandler<T2>>;
            else
                return ConvertToGeneric<T1, T2>(handlers).ToList();
        }

        private List<GenericSyncEventHandler<NullHandler>> ConvertToNormal<T>(List<T> handlers)
            => handlers
                .Select(a => new GenericSyncEventHandler<NullHandler>(a as SyncEventHandler))
                .ToList();

        private List<GenericSyncEventHandler<T2>> ConvertToGeneric<T1, T2>(List<T1> handlers)
            => handlers
                .Select(a => new GenericSyncEventHandler<T2>(a as SyncEventHandler<T2>))
                .ToList();
    }
}