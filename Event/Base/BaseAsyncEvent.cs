using MariGlobals.Event.Concrete;
using MariGlobals.Event.GenericHandlers;
using MariGlobals.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable IDE0060 // Remove unused parameter

namespace MariGlobals.Event.Base
{
    public class BaseAsyncEvent
    {
        private readonly bool IsGeneric;
        protected readonly object _lock = new object();

        internal BaseAsyncEvent(bool isGeneric = false)
        {
            IsGeneric = isGeneric;
        }

        protected Task InvokeAllAsync<T>(List<T> handlers)
            => InvokeAllAsync<T, NullHandler>(handlers);

        protected async Task InvokeAllAsync<T, T2>(List<T> handlers, T2 arg = default)
        {
            var exceptions = MariMemoryExtensions.CreateMemory<Exception>(handlers.Count);

            foreach (var handler in ConvertList<T, T2>(handlers, arg))
            {
                try
                {
                    await handler.InvokeAsync(arg).ConfigureAwait(false);
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

        private IEnumerable<GenericAsyncEventHandler<T2>> ConvertList<T1, T2>(List<T1> handlers, T2 arg)
        {
            if (!IsGeneric)
                return ConvertToNormal(handlers) as IEnumerable<GenericAsyncEventHandler<T2>>;
            else
                return ConvertToGeneric<T1, T2>(handlers);
        }

        private IEnumerable<GenericAsyncEventHandler<NullHandler>> ConvertToNormal<T>(List<T> handlers)
            => handlers
                .Select(a => new GenericAsyncEventHandler<NullHandler>(a as AsyncEventHandler));

        private IEnumerable<GenericAsyncEventHandler<T2>> ConvertToGeneric<T1, T2>(List<T1> handlers)
            => handlers
                .Select(a => new GenericAsyncEventHandler<T2>(a as AsyncEventHandler<T2>));
    }
}