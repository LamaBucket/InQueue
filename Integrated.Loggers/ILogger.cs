namespace Integrated.Loggers;

public static class ILoggerExtensions
{
    public static void LogInformation(this ICustomLogger logger, string message)
    {
        Func<Exception?, string> formatter = (e) => { return message; };
        logger.Log(null, formatter, CustomLogLevel.Information);
    }

    public static void LogError(this ICustomLogger logger, Exception ex)
    {
        logger.Log(ex, ParseException, CustomLogLevel.Information);
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

public enum CustomLogLevel
{
    Information,
    Error
}

public interface ICustomLogger
{
    IDisposable BeginScope<T>(IEnumerable<T> context) where T : ScopeContextItem;

    void Log(Exception? exception, Func<Exception?, string> formatter, CustomLogLevel level);
}