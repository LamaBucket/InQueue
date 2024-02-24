using Newtonsoft.Json;

namespace Integrated.Loggers.State;

public class FileLoggerDataService : ILoggerDataService
{
    public CustomLogLevel Level { get; init; }

    private readonly string _absoluteFilePath;

    public void SaveData(LogMessage message)
    {
        EnsureFileExists();

        string json = JsonConvert.SerializeObject(message);
        string toAppend = Environment.NewLine + json;


        File.AppendAllText(_absoluteFilePath, toAppend);
    }

    private void EnsureFileExists()
    {
        if (!File.Exists(_absoluteFilePath))
        {
            string dir = Path.GetDirectoryName(_absoluteFilePath) ?? throw new Exception("Invalid file path");
            Directory.CreateDirectory(dir);

            File.Create(_absoluteFilePath).Dispose();
        }
    }

    public FileLoggerDataService(CustomLogLevel level, string absoluteFilePath)
    {
        Level = level;
        _absoluteFilePath = absoluteFilePath;
    }
}