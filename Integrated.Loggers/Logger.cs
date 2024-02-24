using Integrated.Loggers.State;

namespace Integrated.Loggers;

public class Logger : ScopedLogger
{
    private Dictionary<LogLevel, ILoggerDataService> _state;

    public override bool IsEnabled(LogLevel logLevel)
    {
        return _state.ContainsKey(logLevel);
    }

    protected override void SaveData(LogLevel level, LogMessage message)
    {
        if (IsEnabled(level))
        {
            _state[level].SaveData(message);
        }
    }

    public void AddState(ILoggerDataService service)
    {
        if (_state.ContainsKey(service.Level))
        {
            throw new Exception();
        }

        _state.Add(service.Level, service);
    }

    public Logger()
    {
        _state = new();
    }
}