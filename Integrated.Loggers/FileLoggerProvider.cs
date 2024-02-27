using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Integrated.Loggers;

public class FileLoggerProvider : ILoggerProvider
{
    private ConcurrentDictionary<string, ILogger> _loggers = new();

    public ILogger CreateLogger(string categoryName)
    {
        ILogger logger = new FileLogger();

        return _loggers.GetOrAdd(categoryName, logger);
    }

    public void Dispose()
    {
    }
}