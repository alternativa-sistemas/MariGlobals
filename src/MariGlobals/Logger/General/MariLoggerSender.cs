using MariGlobals.Event.Concrete;
using MariGlobals.Executor;
using MariGlobals.Logger.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MariGlobals.Logger.General
{
    internal class MariLoggerSender : BaseQueueExecutor<MariEventLogMessage>
    {
        public MariLoggerSender()
        {
            OnLogSender = new NormalEvent<MariEventLogMessage>();
        }

        public event NormalEventHandler<MariEventLogMessage> OnLog
        {
            add => OnLogSender.Register(value);
            remove => OnLogSender.Unregister(value);
        }

        private readonly NormalEvent<MariEventLogMessage> OnLogSender;

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