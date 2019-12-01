namespace MariGlobals.Websocket.Map.Interfaces
{
    public interface IGuildExchange : IExchange
    {
        ulong GuildId { get; set; }
    }
}