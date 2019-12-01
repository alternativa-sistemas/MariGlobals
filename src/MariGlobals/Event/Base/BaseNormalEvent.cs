using MariGlobals.Event.Concrete;
using MariGlobals.Event.GenericHandlers;
using MariGlobals.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable IDE0060 // Remove unused parameter

namespace MariGlobals.Event.Base
{
    public class BaseNormalEvent
    {
        private readonly bool IsGeneric;
        protected readonly object _lock = new object();

        internal BaseNormalEvent(bool isGeneric = false)
        {
            IsGeneric = isGeneric;
        }

        protected void InvokeAll<T>(List<T> handlers)
            => InvokeAll<T, NullHandler>(handlers);

        protected void InvokeAll<T, T2>(List<T> handlers, T2 arg = default)
        {
            var exceptions = MariMemoryExtensions.CreateMemory<Exception>(handlers.Count);

            foreach (var handler in ConvertList<T, T2>(handlers, arg))
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

        private IEnumerable<GenericNormalEventHandler<T2>> ConvertList<T1, T2>(List<T1> handlers, T2 arg)
        {
            if (!IsGeneric)
                return ConvertToNormal(handlers) as IEnumerable<GenericNormalEventHandler<T2>>;
            else
                return ConvertToGeneric<T1, T2>(handlers);
        }

        private IEnumerable<GenericNormalEventHandler<NullHandler>> ConvertToNormal<T>(List<T> handlers)
            => handlers
                .Select(a => new GenericNormalEventHandler<NullHandler>(a as NormalEventHandler));

        private IEnumerable<GenericNormalEventHandler<T2>> ConvertToGeneric<T1, T2>(List<T1> handlers)
            => handlers
                .Select(a => new GenericNormalEventHandler<T2>(a as NormalEventHandler<T2>));
    }
}