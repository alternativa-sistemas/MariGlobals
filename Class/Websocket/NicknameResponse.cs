using MariGlobals.Interfaces.Websocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace MariGlobals.Class.Websocket
{
    public class NicknameResponse : IExchange, IGuildExchange, INicknameExchange, IResponse
    {
        public NicknameResponse()
        {
        }

        public NicknameResponse(INicknameExchange request, string nickname = null)
        {
            SetRequestResponse(request);
            StatusCode = 200;
            Message = "Ok";
            Nickname = nickname ?? request.Nickname;
        }

        public NicknameResponse(Exception ex, INicknameExchange request)
        {
            SetRequestResponse(request);
            StatusCode = 500;
            Message = "Internal Server Error";
            Exception = ex;
        }

        private void SetRequestResponse(INicknameExchange request)
        {
            RequestId = request.RequestId;
            GuildId = request.GuildId;
        }

        public string RequestId { get; set; }

        public ulong GuildId { get; set; }

        public string Nickname { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }
    }
}