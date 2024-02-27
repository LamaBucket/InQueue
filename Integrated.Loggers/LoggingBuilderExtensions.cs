using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Integrated.Loggers;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>()
        );

        return builder;
    }

}