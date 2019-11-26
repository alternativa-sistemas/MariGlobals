using MariGlobals.Websocket.Map.Enums;
using MariGlobals.Websocket.Map.Interfaces;
using System;

namespace MariGlobals.Websocket.Map.Classes
{
    public class PrefixRequest : IPrefixExchange
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString();

        public string Prefix { get; set; }

        public ulong GuildId { get; set; }

        public PrefixRequestType RequestType { get; set; } = PrefixRequestType.PrefixUpdate;
    }
}