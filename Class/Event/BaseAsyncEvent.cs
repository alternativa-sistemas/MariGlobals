using MariGlobals.Class.Utils;
using MariGlobals.Structs.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariGlobals.Class.Event
{
    public class BaseAsyncEvent
    {
        private readonly bool IsGeneric;
        protected readonly object _lock = new object();

        public BaseAsyncEvent(bool isGeneric = false)
        {
            IsGeneric = isGeneric;
        }

        protected Task InvokeAllAsync<T>(List<T> handlers)
            => InvokeAllAsync<T, NullHandler>(handlers);

        protected async Task InvokeAllAsync<T, T2>(List<T> handlers, T2 arg = default)
        {
            var exceptions = new Memory<Exception>(new Exception[handlers.Count]);

            foreach (var handler in ConvertList(handlers, arg))
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

            if (exceptions.Span.Length > 0)
                throw new AggregateException(
                    "Exceptions occured within one or more event handlers. " +
                    "Check InnerExceptions for details.", exceptions.Span.ToArray());
        }

        private IEnumerable<GenericAsyncEventHandler<T2>> ConvertList<T1, T2>(List<T1> handlers, T2 obj)
        {
            if (!IsGeneric)
                return ConvertToNormal(handlers) as List<GenericAsyncEventHandler<T2>>;
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