namespace Integrated.Loggers;

public class LogMessage
{
    public IEnumerable<ScopeContextItem> Context { get; init; }

    public string Message { get; init; }

    public LogMessage(IEnumerable<ScopeContextItem> context, string message)
    {
        Context = context;
        Message = message;
    }
}