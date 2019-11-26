using MariGlobals.Event.Concrete;

namespace MariGlobals.Event.GenericHandlers
{
    internal readonly struct GenericNormalEventHandler<T>
    {
        private readonly NormalEventHandler NormalHandler;
        private readonly NormalEventHandler<T> GenericHandler;
        private readonly bool IsGeneric;

        internal GenericNormalEventHandler(NormalEventHandler handler)
        {
            NormalHandler = handler;
            GenericHandler = default;
            IsGeneric = false;
        }

        internal GenericNormalEventHandler(NormalEventHandler<T> handler)
        {
            NormalHandler = default;
            GenericHandler = handler;
            IsGeneric = true;
        }

        public void Invoke(T obj)
        {
            if (!IsGeneric)
                NormalHandler();
            else
                GenericHandler(obj);
        }
    }
}