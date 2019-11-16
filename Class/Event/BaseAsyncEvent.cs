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
        public Task InvokeAllAsync<T>(List<T> handlers)
            => InvokeAllAsync<T, NullHandler>(handlers);

        public async Task InvokeAllAsync<T, T2>(List<T> handlers, T2 arg = default)
        {
            var exceptions = new List<Exception>(handlers.Count);

            foreach (var handler in ConvertList(handlers, arg))
            {
                try
                {
                    await handler.InvokeAsync(arg).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
                throw new AggregateException(
                    "Exceptions occured within one or more event handlers. " +
                    "Check InnerExceptions for details.", exceptions);
        }

        private List<GenericAsyncEventHandler<T2>> ConvertList<T1, T2>(List<T1> handlers, T2 obj)
        {
            if (typeof(T2).IsEquivalentTo(typeof(NullHandler)))
                return ConvertToNormal(handlers) as List<GenericAsyncEventHandler<T2>>;
            else
                return ConvertToGeneric(handlers, obj);
        }

        private List<GenericAsyncEventHandler<NullHandler>> ConvertToNormal<T>(List<T> handlers)
            => handlers
                .Select(a => new GenericAsyncEventHandler<NullHandler>(a as AsyncEventHandler))
                .ToList();

        private List<GenericAsyncEventHandler<T2>> ConvertToGeneric<T1, T2>(List<T1> handlers, T2 obj)
            => handlers
                .Select(a => new GenericAsyncEventHandler<T2>(a as AsyncEventHandler<T2>))
                .ToList();
    }
}