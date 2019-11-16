using MariGlobals.Class.Event;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MariGlobals.Structs.Event
{
    internal readonly struct GenericAsyncEventHandler<T>
    {
        private readonly AsyncEventHandler NormalHandler;
        private readonly AsyncEventHandler<T> GenericHandler;
        private readonly bool IsGeneric;

        internal GenericAsyncEventHandler(AsyncEventHandler handler)
        {
            NormalHandler = handler;
            GenericHandler = default;
            IsGeneric = false;
        }

        internal GenericAsyncEventHandler(AsyncEventHandler<T> handler)
        {
            NormalHandler = default;
            GenericHandler = handler;
            IsGeneric = true;
        }

        public async Task InvokeAsync(T obj)
        {
            if (!IsGeneric)
                await NormalHandler().ConfigureAwait(false);
            else
                await GenericHandler(obj).ConfigureAwait(false);
        }
    }
}