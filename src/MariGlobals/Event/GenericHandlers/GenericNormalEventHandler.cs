namespace MariGlobals.Events.GenericHandlers
{
    internal readonly struct GenericSyncEventHandler<T>
    {
        private readonly SyncEventHandler NormalHandler;
        private readonly SyncEventHandler<T> GenericHandler;
        private readonly bool IsGeneric;

        internal GenericSyncEventHandler(SyncEventHandler handler)
        {
            NormalHandler = handler;
            GenericHandler = default;
            IsGeneric = false;
        }

        internal GenericSyncEventHandler(SyncEventHandler<T> handler)
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