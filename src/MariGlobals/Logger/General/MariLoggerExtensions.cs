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
        /// <param name="section">The <see cref="IConfiguration" /> with the loglevels config. </param>
        /// <param name="config">The <see cref="MariLoggerConfig" /> with Mari's logger configuration.</param>
        /// <returns>The current service container.</returns>
        public static ILoggingBuilder AddMariLogger(
            this ILoggingBuilder loggingBuilder,
            IConfiguration section,
            MariLoggerConfig config)
            => loggingBuilder.AddProvider(new MariLoggerProvider(config,
                section
                    ?.GetSection("Logging")
                    ?.GetSection("LogLevel")));

        /// <summary>
        /// Add the MariLogger to the Service container.
        /// </summary>
        /// <param name="loggingBuilder">The <see cref="ILoggingBuilder" />.</param>
        /// <param name="section">The <see cref="IConfiguration" /> with the loglevels config. </param>
        /// <returns>The current service container.</returns>
        public static ILoggingBuilder AddMariLogger(this ILoggingBuilder loggingBuilder, IConfiguration section)
            => loggingBuilder.AddMariLogger(section, new MariLoggerConfig());

        /// <summary>
        /// Add the MariLogger to the Service container.
        /// </summary>
        /// <param name="loggingBuilder">The <see cref="ILoggingBuilder" />.</param>
        /// <param name="section">The <see cref="IConfiguration" /> with the loglevels config. </param>
        /// <param name="configure">An action of the <see cref="MariLoggerConfig" /> with Mari's logger configuration.</param>
        /// <returns>The current service container.</returns>
        public static ILoggingBuilder AddMariLogger(
            this ILoggingBuilder loggingBuilder,
            IConfiguration section,
            Action<MariLoggerConfig> configure)
            => loggingBuilder.AddMariLogger(section, configure.ConfigureConfigEvent());

        /// <summary>
        /// Add the MariLogger to the Service container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="section">The <see cref="IConfiguration" /> with the loglevels config. </param>
        /// <param name="config">The <see cref="MariLoggerConfig" /> with Mari's logger configuration.</param>
        /// <returns>The current service container.</returns>    
        public static IServiceCollection AddMariLogger(
            this IServiceCollection services,
            IConfiguration section,
            MariLoggerConfig config)
            => services.AddLogging(a => a.AddMariLogger(section, config));

        /// <summary>
        /// Add the MariLogger to the Service container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="section">The <see cref="IConfiguration" /> with the loglevels config. </param>
        /// <returns>The current service container.</returns>    
        public static IServiceCollection AddMariLogger(this IServiceCollection services, IConfiguration section)
            => services.AddMariLogger(section, new MariLoggerConfig());

        /// <summary>
        /// Add the MariLogger to the Service container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="section">The <see cref="IConfiguration" /> with the loglevels config. </param>
        /// <param name="configure">An action of the <see cref="MariLoggerConfig" /> with Mari's logger configuration.</param>
        /// <returns>The current service container.</returns>
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