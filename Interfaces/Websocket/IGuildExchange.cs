using System;
using System.Collections.Generic;
using System.Text;

namespace MariGlobals.Interfaces.Websocket
{
    public interface IGuildExchange : IExchange
    {
        ulong GuildId { get; set; }
    }
}