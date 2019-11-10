using System;
using System.Collections.Generic;
using System.Text;

namespace MariGlobals.Interfaces.Websocket
{
    public interface IPrefixExchange : IExchange, IGuildExchange
    {
        string Prefix { get; set; }
    }
}