using MariGlobals.Event.Concrete;
using System.Threading.Tasks;

namespace MariGlobals.Event.GenericHandlers
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