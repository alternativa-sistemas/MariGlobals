using Discord;
using MariGlobals.Logger.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Color = System.Drawing.Color;

namespace MariGlobals.Logger.General
{
    public static class MariLoggerExtensions
    {
        public static ILoggingBuilder AddMariLogger
            (this ILoggingBuilder loggingBuilder, IConfiguration section, MariLoggerConfig config)
            => loggingBuilder.AddProvider(new MariLoggerProvider(config,
                section
                    .GetSection("Logging")
                    .GetSection("LogLevel")));

        public static ILoggingBuilder AddMariLogger(this ILoggingBuilder loggingBuilder, IConfiguration section)
            => loggingBuilder.AddMariLogger(section, new MariLoggerConfig());

        public static ILoggingBuilder AddMariLogger
            (this ILoggingBuilder loggingBuilder, IConfiguration section, Action<MariLoggerConfig> configure)
            => loggingBuilder.AddMariLogger(section, configure.ConfigureConfigEvent());

        public static IServiceCollection AddMariLogger
            (this IServiceCollection services, IConfiguration section, MariLoggerConfig config)
            => services.AddLogging(a => a.AddMariLogger(section, config));

        public static IServiceCollection AddMariLogger(this IServiceCollection services, IConfiguration section)
            => services.AddMariLogger(section, new MariLoggerConfig());

        public static IServiceCollection AddMariLogger
            (this IServiceCollection services, IConfiguration section, Action<MariLoggerConfig> configure)
            => services.AddMariLogger(section, configure.ConfigureConfigEvent());

        private static MariLoggerConfig ConfigureConfigEvent(this Action<MariLoggerConfig> configure)
        {
            var config = new MariLoggerConfig();
            configure(config);
            return config;
        }

        public static LogLevel ConvertLevel(this LogSeverity severity)
        {
            switch (severity)
            {
                default:
                    throw new InvalidOperationException($"Invalid {nameof(LogSeverity)} type: {severity}");

                case LogSeverity.Critical:
                    return LogLevel.Critical;

                case LogSeverity.Debug:
                    return LogLevel.Trace;

                case LogSeverity.Error:
                    return LogLevel.Error;

                case LogSeverity.Info:
                    return LogLevel.Information;

                case LogSeverity.Verbose:
                    return LogLevel.Debug;

                case LogSeverity.Warning:
                    return LogLevel.Warning;
            }
        }

        public static (Color Color, string Abbreviation) LogLevelInfo(this LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Information:
                    return (Color.SpringGreen, "INFO");

                case LogLevel.Debug:
                    return (Color.MediumPurple, "DBUG");

                case LogLevel.Trace:
                    return (Color.MediumPurple, "TRCE");

                case LogLevel.Critical:
                    return (Color.Crimson, "CRIT");

                case LogLevel.Error:
                    return (Color.Crimson, "EROR");

                case LogLevel.Warning:
                    return (Color.Orange, "WARN");

                default:
                    return (Color.Tomato, "UKNW");
            }
        }
    }
}