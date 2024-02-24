namespace Integrated.Loggers;

public static class ILoggerExtensions
{
    public static void LogInformation(this ILogger logger, string message)
    {
        Func<Exception?, string> formatter = (e) => { return message; };
        logger.Log(null, formatter, LogLevel.Information);
    }

    public static void LogError(this ILogger logger, Exception ex)
    {
        logger.Log(ex, ParseException, LogLevel.Information);
    }

    private static string ParseException(Exception? ex)
    {
        if (ex is null)
            return "An Error Occured.";

        string message = GetInnerException(ex);

        return message;
    }

    private static string GetInnerException(Exception ex)
    {
        if (ex.InnerException is not null)
        {
            return GetInnerException(ex.InnerException);
        }

        return ex.Message;
    }

}

public enum LogLevel
{
    Information,
    Error
}

public interface ILogger
{
    IDisposable BeginScope<T>(IEnumerable<T> context) where T : ScopeContextItem;

    void Log(Exception? exception, Func<Exception?, string> formatter, LogLevel level);
}