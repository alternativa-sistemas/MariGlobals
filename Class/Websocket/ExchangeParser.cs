using MariGlobals.Interfaces.Websocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MariGlobals.Class.Websocket
{
    public static class ExchangeParser
    {
        public static T Deserialize<T>(string json)
            where T : class, IExchange, IGuildExchange
            => JsonConvert.DeserializeObject<T>(json);

        public static string Serialize<T>(this T obj)
            where T : class, IExchange, IGuildExchange
            => JsonConvert.SerializeObject(obj);
    }
}