using MariGlobals.Events;

namespace MariGlobals.Logger
{
    /// <summary>
    /// General configuration for MariGlobals.Logger.
    /// </summary>
    public class MariLoggerConfig
    {
        /// <summary>
        /// An event fired when any log message is created.
        /// </summary>
        public event NormalEventHandler<MariEventLogMessage> OnLog
        {
            add => SendLog.Register(value);
            remove => SendLog.Unregister(value);
        }

        /// <summary>
        /// Gets or sets if the logs will be written in console.
        /// Default is false.
        /// </summary>
        public bool EnableWriter { get; set; } = false;

        internal readonly NormalEvent<MariEventLogMessage> SendLog = new NormalEvent<MariEventLogMessage>();
    }
}