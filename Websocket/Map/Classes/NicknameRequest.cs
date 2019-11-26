using MariGlobals.Websocket.Map.Enums;
using MariGlobals.Websocket.Map.Interfaces;
using System;

namespace MariGlobals.Websocket.Map.Classes
{
    public class NicknameRequest : INicknameExchange
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString();

        public ulong GuildId { get; set; }

        public string Nickname { get; set; }

        public NicknameRequestType RequestType { get; set; } = NicknameRequestType.NicknameUpdate;
    }
}