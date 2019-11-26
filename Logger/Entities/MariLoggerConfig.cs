using MariGlobals.Event.Concrete;

namespace MariGlobals.Logger.Entities
{
    public class MariLoggerConfig
    {
        public event NormalEventHandler<MariEventLogMessage> OnLog
        {
            add => SendLog.Register(value);
            remove => SendLog.Unregister(value);
        }

        public bool EnableWriter { get; set; } = false;

        internal readonly NormalEvent<MariEventLogMessage> SendLog = new NormalEvent<MariEventLogMessage>();
    }
}