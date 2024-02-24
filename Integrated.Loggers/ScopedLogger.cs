namespace Integrated.Loggers;

public abstract class ScopedLogger : ILogger
{
    private Dictionary<ScopeKey, IEnumerable<ScopeContextItem>> _scope;

    private readonly object _lock = new();


    public IDisposable BeginScope<T>(IEnumerable<T> context) where T : ScopeContextItem
    {
        return CreateScope(context);
    }

    private ScopeKey CreateScope(IEnumerable<ScopeContextItem> context)
    {
        ScopeKey key = new(this);

        _scope.Add(key, context);

        return key;
    }


    internal void RemoveFromScope(ScopeKey key)
    {
        _scope.Remove(key);
    }


    public abstract bool IsEnabled(LogLevel logLevel);


    public void Log(Exception? exception, Func<Exception?, string> formatter, LogLevel logLevel)
    {
        if (IsEnabled(logLevel))
        {
            lock (_lock)
            {
                string message = formatter(exception);
                var context = GetCurrentContext();

                LogMessage log = new(context, message);

                SaveData(logLevel, log);
            }
        }
    }

    private IEnumerable<ScopeContextItem> GetCurrentContext()
    {
        List<ScopeContextItem> result = new();

        foreach (var scopeCollection in _scope.Values)
        {
            result.AddRange(scopeCollection);
        }

        return result;
    }

    protected abstract void SaveData(LogLevel level, LogMessage message);

    public ScopedLogger()
    {
        _scope = new();
    }
}
