using System.Runtime.Loader;
using SQA.EntityFramework.Model;

namespace SQA.EntityFramework.Services.DataTable;

public class DataItemEditService<T> : IDataItemEditService<T> where T : DataTableObject
{
    private readonly SQADbContextFactory _dbContextFactory;

    public async Task Create(T entity)
    {
        using (var dbContext = _dbContextFactory.CreateDbContext())
        {
            await dbContext.Set<T>().AddAsync(entity);

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task Delete(T entity)
    {
        using (var dbContext = _dbContextFactory.CreateDbContext())
        {
            dbContext.Set<T>().Remove(entity);

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task Update(T entity)
    {
        using (var dbContext = _dbContextFactory.CreateDbContext())
        {
            dbContext.Set<T>().Update(entity);

            await dbContext.SaveChangesAsync();
        }
    }

    public DataItemEditService(SQADbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
}