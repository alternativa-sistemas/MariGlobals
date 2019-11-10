using System;
using System.Collections.Generic;
using System.Text;

namespace MariGlobals.Interfaces.Websocket
{
    public interface INicknameExchange : IExchange, IGuildExchange
    {
        string Nickname { get; set; }
    }
}