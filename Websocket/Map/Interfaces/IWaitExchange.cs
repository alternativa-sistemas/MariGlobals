namespace MariGlobals.Websocket.Map.Interfaces
{
    public interface IWaitExchange : IExchange
    {
        ulong WaitId { get; set; }
    }
}