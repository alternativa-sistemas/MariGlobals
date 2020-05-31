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