using MariGlobals.Events;
using MariGlobals.Executor;

namespace MariGlobals.Logger.General
{
    internal class MariLoggerSender : BaseQueueExecutor<MariEventLogMessage>
    {
        public MariLoggerSender()
        {
            OnLogSender = new SyncEvent<MariEventLogMessage>();
        }

        public event SyncEventHandler<MariEventLogMessage> OnLog
        {
            add => OnLogSender.Register(value);
            remove => OnLogSender.Unregister(value);
        }

        private readonly SyncEvent<MariEventLogMessage> OnLogSender;

        public void AddLog(MariEventLogMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.Message) || string.IsNullOrWhiteSpace(message.SectionName))
                return;

            AddObject(message);
        }

        protected override void Action(MariEventLogMessage message)
        {
            OnLogSender.Invoke(message);
        }
    }
}