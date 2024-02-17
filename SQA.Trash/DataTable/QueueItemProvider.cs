using Microsoft.EntityFrameworkCore;
using SQA.EntityFramework.Model;

namespace SQA.EntityFramework.Services.DataTable;

public class QueueItemProvider : IQueueItemProvider
{
    private readonly SQADbContextFactory _dbContextFactory;


    public async Task<QueueItem> Get(int id)
    {
        using (var dbContext = _dbContextFactory.CreateDbContext())
        {
            var item = await dbContext.Set<QueueItem>().FirstOrDefaultAsync(x => x.QueueId == id);

            if (item is null)
                throw new Exception();

            return item;
        }
    }

    public async Task<IEnumerable<QueueItem>> GetAll()
    {
        using (var dbContext = _dbContextFactory.CreateDbContext())
        {
            var items = await dbContext.Set<QueueItem>().ToListAsync();

            return items;
        }
    }

    public QueueItemProvider(SQADbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
}