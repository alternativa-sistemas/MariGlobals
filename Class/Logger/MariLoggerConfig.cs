using MariGlobals.Class.Event;
using MariGlobals.Structs.Logger;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MariGlobals.Class.Logger
{
    public class MariLoggerConfig
    {
        public event NormalEventHandler<MariEventLogMessage> OnLog
        {
            add => SendLog.Register(value);
            remove => SendLog.Unregister(value);
        }

        public string App { get; set; }

        public bool EnableWriter { get; set; } = false;

        internal readonly NormalEvent<MariEventLogMessage> SendLog = new NormalEvent<MariEventLogMessage>();
    }
}