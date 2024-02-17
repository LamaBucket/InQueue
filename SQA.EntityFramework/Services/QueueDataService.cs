using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SQA.Domain;
using SQA.Domain.Services;
using SQA.Domain.Services.Data;
using SQA.EntityFramework.Model;

namespace SQA.EntityFramework.Services;

public class QueueDataService : IQueueDataService
{
    private readonly SQADbContextFactory _contextFactory;

    private readonly IQueueBuilder _queueBuilder;


    public async Task<Queue> Get(int id)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            var queueItem = await dbContext.Set<QueueItem>().Include(x => x.Records).FirstOrDefaultAsync(x => x.QueueId == id);

            if (queueItem is null || queueItem.Records is null)
                throw new Exception();

            var queueRecords = queueItem.Records.Select(x => new QueueRecord(x.Position, x.Username));

            var queueInfo = _queueBuilder.CreateQueueInfo(queueItem.QueueId, queueItem.QueueName, DateTime.Now);
            var queue = _queueBuilder.CreateQueue(queueInfo, queueItem.CurrentPosition, queueItem.IsInfinite, queueRecords);

            return queue;
        }
    }

    public async Task<IEnumerable<QueueInfo>> GetForUser(string username)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            var queueItems = await dbContext.Set<QueueItem>().Include(x => x.Records!.Where(r => r.Username == username)).ToListAsync();

            var queues = queueItems.Select(x => _queueBuilder.CreateQueueInfo(x.QueueId, x.QueueName, DateTime.Now));

            return queues;
        }
    }

    public async Task Create(string queueName, bool isInfinite)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            var queueItem = new QueueItem(queueName, isInfinite);

            await dbContext.Set<QueueItem>().AddAsync(queueItem);

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task Update(Queue queue)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            QueueItem item = new(queue.QueueInfo.Id, queue.QueueInfo.Name, queue.IsInfinite, queue.CurrentPosition);

            var records = queue.Records.Select(x => new QueueRecordItem(x.Username, queue.QueueInfo.Id, x.Position)).ToList();

            item.Records = records;

            await Delete(item.QueueId);

            await dbContext.Set<QueueItem>().AddAsync(item);

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task Delete(int id)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            var queueItem = await dbContext.Set<QueueItem>().FirstOrDefaultAsync(x => x.QueueId == id);

            if (queueItem is null)
                throw new Exception();

            dbContext.Set<QueueItem>().Remove(queueItem);

            await dbContext.SaveChangesAsync();
        }
    }


    public QueueDataService(SQADbContextFactory contextFactory, IQueueBuilder queueBuilder)
    {
        _contextFactory = contextFactory;
        _queueBuilder = queueBuilder;
    }
}