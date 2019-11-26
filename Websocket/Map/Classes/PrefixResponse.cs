using MariGlobals.Websocket.Map.Interfaces;
using System;

namespace MariGlobals.Websocket.Map.Classes
{
    public class PrefixResponse : IPrefixExchange, IResponse
    {
        public PrefixResponse()
        {
        }

        public PrefixResponse(IPrefixExchange request, string prefix = null)
        {
            SetRequestResponse(request);
            StatusCode = 200;
            Message = "Ok";
            Prefix = prefix ?? request.Prefix;
        }

        public PrefixResponse(Exception ex, IPrefixExchange request)
        {
            SetRequestResponse(request);
            StatusCode = 500;
            Message = "Internal Server Error";
            Exception = ex;
        }

        private void SetRequestResponse(IPrefixExchange request)
        {
            RequestId = request.RequestId;
            GuildId = request.GuildId;
        }

        public string RequestId { get; set; }

        public ulong GuildId { get; set; }

        public string Prefix { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }
    }
}