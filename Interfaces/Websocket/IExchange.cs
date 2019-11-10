using System;
using System.Collections.Generic;
using System.Text;

namespace MariGlobals.Interfaces.Websocket
{
    public interface IExchange
    {
        string RequestId { get; set; }
    }
}