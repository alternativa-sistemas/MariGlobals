using MariGlobals.Websocket.Map.Enums;
using MariGlobals.Websocket.Map.Interfaces;
using System;

namespace MariGlobals.Websocket.Map.Classes
{
    public class WaitRequest : IWaitExchange
    {
        public ulong WaitId { get; set; }

        public string RequestId { get; set; } = Guid.NewGuid().ToString();

        public WaitRequestType RequestType { get; set; } = WaitRequestType.WaitGuild;
    }
}