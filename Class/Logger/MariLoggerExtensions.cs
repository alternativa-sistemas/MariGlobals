using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MariGlobals.Class.Logger
{
    public static class MariLoggerExtensions
    {
        public static ILoggingBuilder AddMariLogger(this ILoggingBuilder loggingBuilder, MariLoggerConfig config)
            => loggingBuilder.AddProvider(new MariLoggerProvider(config));

        public static ILoggingBuilder AddMariLogger(this ILoggingBuilder loggingBuilder)
            => loggingBuilder.AddMariLogger(new MariLoggerConfig());

        public static ILoggingBuilder AddMariLogger(this ILoggingBuilder loggingBuilder, Action<MariLoggerConfig> configure)
            => loggingBuilder.AddMariLogger(configure.ConfigureConfig());

        public static IServiceCollection AddMariLogger(this IServiceCollection services, MariLoggerConfig config)
            => services.AddLogging(a => a.AddMariLogger(config));

        public static IServiceCollection AddMariLogger(this IServiceCollection services)
            => services.AddMariLogger(new MariLoggerConfig());

        public static IServiceCollection AddMariLogger(this IServiceCollection services, Action<MariLoggerConfig> configure)
            => services.AddMariLogger(configure.ConfigureConfig());

        private static MariLoggerConfig ConfigureConfig(this Action<MariLoggerConfig> configure)
        {
            var config = new MariLoggerConfig();
            configure(config);
            return config;
        }
    }
}