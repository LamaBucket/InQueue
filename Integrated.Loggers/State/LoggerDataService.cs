namespace Integrated.Loggers.State;

public interface ILoggerDataService
{
    CustomLogLevel Level { get; init; }

    void SaveData(LogMessage message);
}