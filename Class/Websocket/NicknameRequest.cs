using MariGlobals.Enums.Websocket;
using MariGlobals.Interfaces.Websocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace MariGlobals.Class.Websocket
{
    public class NicknameRequest : IExchange, IGuildExchange, INicknameExchange
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString();

        public ulong GuildId { get; set; }

        public string Nickname { get; set; }

        public NicknameRequestType RequestType { get; set; } = NicknameRequestType.NicknameUpdate;
    }
}