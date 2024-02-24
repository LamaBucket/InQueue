namespace Integrated.Loggers.State;

public interface ILoggerDataService
{
    LogLevel Level { get; init; }

    void SaveData(LogMessage message);
}