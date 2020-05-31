using MariGlobals.Logger.General;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Color = System.Drawing.Color;

namespace MariGlobals.Logger
{
    /// <summary>
    /// Extensions for MariGlobals.Logger.
    /// </summary>
    public static class MariLoggerExtensions
    {
        /// <summary>
        /// Add the MariLogger to the Service container.
        /// </summary>
        /// <param name="loggingBuilder">The <see cref="ILoggingBuilder" />.</param>
        /// <param name="section">The <see cref="IConfiguration" </param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddMariLogger(
            this ILoggingBuilder loggingBuilder,
            IConfiguration section,
            MariLoggerConfig config)
            => loggingBuilder.AddProvider(new MariLoggerProvider(config,
                section
                    ?.GetSection("Logging")
                    ?.GetSection("LogLevel")));

        public static ILoggingBuilder AddMariLogger(this ILoggingBuilder loggingBuilder, IConfiguration section)
            => loggingBuilder.AddMariLogger(section, new MariLoggerConfig());

        public static ILoggingBuilder AddMariLogger(
            this ILoggingBuilder loggingBuilder,
            IConfiguration section,
            Action<MariLoggerConfig> configure)
            => loggingBuilder.AddMariLogger(section, configure.ConfigureConfigEvent());

        public static IServiceCollection AddMariLogger(
            this IServiceCollection services,
            IConfiguration section,
            MariLoggerConfig config)
            => services.AddLogging(a => a.AddMariLogger(section, config));

        public static IServiceCollection AddMariLogger(this IServiceCollection services, IConfiguration section)
            => services.AddMariLogger(section, new MariLoggerConfig());

        public static IServiceCollection AddMariLogger(
            this IServiceCollection services,
            IConfiguration section,
            Action<MariLoggerConfig> configure)
            => services.AddMariLogger(section, configure.ConfigureConfigEvent());

        private static MariLoggerConfig ConfigureConfigEvent(this Action<MariLoggerConfig> configure)
        {
            var config = new MariLoggerConfig();
            configure(config);
            return config;
        }

        internal static (Color Color, string Abbreviation) LogLevelInfo(this LogLevel logLevel)
            => logLevel switch
            {
                LogLevel.Information => (Color.SpringGreen, "INFO"),
                LogLevel.Debug => (Color.MediumPurple, "DBUG"),
                LogLevel.Trace => (Color.MediumPurple, "TRCE"),
                LogLevel.Critical => (Color.Crimson, "CRIT"),
                LogLevel.Error => (Color.Crimson, "EROR"),
                LogLevel.Warning => (Color.Orange, "WARN"),
                _ => (Color.Tomato, "UKNW"),
            };
    }
}