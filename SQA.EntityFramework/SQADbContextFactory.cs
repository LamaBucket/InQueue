using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SQA.EntityFramework;

public class SQADbContextFactory : IDesignTimeDbContextFactory<SQADbContext>
{
    public static string ConnectionString 
    {
        get
        {
            string filePath = "DbPath.txt";

            EnsureFileExists(filePath);

            var dbPath = File.ReadAllText(filePath);

            if(String.IsNullOrEmpty(dbPath))
                throw new Exception($"Db Error! File Is Empty: {dbPath}");

            return dbPath;
        }
    }

    private static void EnsureFileExists(string path)
    {
        if(!File.Exists(path))
        {
            File.Create(path).Dispose();
        }
    }


    public SQADbContext CreateDbContext(string[] args = null!)
    {
        DbContextOptionsBuilder options = new();

        options.UseSqlServer(ConnectionString);

        SQADbContext context = new(options.Options);

        return context;
    }
}