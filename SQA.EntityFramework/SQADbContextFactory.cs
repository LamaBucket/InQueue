using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SQA.EntityFramework;

public class SQADbContextFactory : IDesignTimeDbContextFactory<SQADbContext>
{
    public static readonly string ConnectionString = "Data Source=test.db;";

    public SQADbContext CreateDbContext(string[] args = null!)
    {
        DbContextOptionsBuilder options = new();

        options.UseSqlite(ConnectionString);

        SQADbContext context = new(options.Options);

        return context;
    }
}