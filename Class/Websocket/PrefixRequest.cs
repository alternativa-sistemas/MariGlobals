using MariGlobals.Enums.Websocket;
using MariGlobals.Interfaces.Websocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace MariGlobals.Class.Websocket
{
    public class PrefixRequest : IExchange, IGuildExchange, IPrefixExchange
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString();

        public string Prefix { get; set; }

        public ulong GuildId { get; set; }

        public PrefixRequestType RequestType { get; set; } = PrefixRequestType.PrefixUpdate;
    }
}