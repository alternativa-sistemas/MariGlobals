using System;
using System.Collections.Generic;
using System.Text;

namespace MariGlobals.Interfaces.Websocket
{
    public interface IResponse : IExchange, IGuildExchange
    {
        int StatusCode { get; set; }

        string Message { get; set; }

        Exception Exception { get; set; }
    }
}