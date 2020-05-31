using System;
using System.Collections.Generic;
using System.Linq;
using MariGlobals.Events.GenericHandlers;
using MariGlobals.Extensions;

#pragma warning disable IDE0060 // Remove unused parameter

namespace MariGlobals.Events
{
    /// <summary>
    /// Base class for sync events.
    /// </summary>
    public class BaseNormalEvent
    {
        private readonly bool IsGeneric;

        /// <summary>
        /// An <see cref="object" /> to use for lock threads in this event.
        /// </summary>
        protected readonly object _lock = new object();

        internal BaseNormalEvent(bool isGeneric = false)
        {
            IsGeneric = isGeneric;
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
                try
                {
                    handler.Invoke(arg);
                }
                catch (Exception ex)
                {
                    exceptions.TryAdd(ex);
                }
            }

            if (exceptions.HasContent())
                throw new AggregateException(
                    "Exceptions occured within one or more event handlers. " +
                    "Check InnerExceptions for details.", exceptions.ToArray());
        }

        private List<GenericNormalEventHandler<T2>> ConvertList<T1, T2>(List<T1> handlers, T2 arg)
        {
            if (!IsGeneric)
                return ConvertToNormal(handlers) as List<GenericNormalEventHandler<T2>>;
            else
                return ConvertToGeneric<T1, T2>(handlers).ToList();
        }

        private List<GenericNormalEventHandler<NullHandler>> ConvertToNormal<T>(List<T> handlers)
            => handlers
                .Select(a => new GenericNormalEventHandler<NullHandler>(a as NormalEventHandler))
                .ToList();

        private List<GenericNormalEventHandler<T2>> ConvertToGeneric<T1, T2>(List<T1> handlers)
            => handlers
                .Select(a => new GenericNormalEventHandler<T2>(a as NormalEventHandler<T2>))
                .ToList();
    }
}