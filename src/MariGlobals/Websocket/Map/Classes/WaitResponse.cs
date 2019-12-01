using MariGlobals.Websocket.Map.Interfaces;
using System;

namespace MariGlobals.Websocket.Map.Classes
{
    public class WaitResponse : IWaitExchange, IResponse
    {
        public WaitResponse()
        {
        }

        public WaitResponse(IWaitExchange request, string prefix = null)
        {
            SetRequestResponse(request);
            StatusCode = 200;
            Message = "Ok";
        }

        public WaitResponse(Exception ex, IWaitExchange request)
        {
            SetRequestResponse(request);
            StatusCode = 500;
            Message = "Internal Server Error";
            Exception = ex;
        }

        private void SetRequestResponse(IWaitExchange request)
        {
            RequestId = request.RequestId;
            WaitId = request.WaitId;
        }

        public ulong WaitId { get; set; }

        public string RequestId { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public bool Finalized { get; set; } = true;
    }
}