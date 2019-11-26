using MariGlobals.Websocket.Map.Interfaces;
using Newtonsoft.Json;

namespace MariGlobals.Websocket.General
{
    public static class ExchangeParser
    {
        public static T Deserialize<T>(string json)
            where T : class, IExchange
            => JsonConvert.DeserializeObject<T>(json);

        public static string Serialize<T>(this T obj)
            where T : class, IExchange
            => JsonConvert.SerializeObject(obj);
    }
}