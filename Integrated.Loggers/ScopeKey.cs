using System;

namespace Integrated.Loggers;

public class ScopeKey : IDisposable
{
    private readonly ScopedLogger _logger;

    public void Dispose()
    {
        _logger.RemoveFromScope(this);
    }

    public ScopeKey(ScopedLogger logger)
    {
        _logger = logger;
    }
}