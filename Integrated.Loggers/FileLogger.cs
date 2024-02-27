using Microsoft.Extensions.Logging;

namespace Integrated.Loggers;

public class FileLogger : ILogger, IDisposable
{


    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return this;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string message = formatter(state, exception);

        File.AppendAllText("output.log", message + Environment.NewLine);
    }

    public void Dispose()
    {
    }
}