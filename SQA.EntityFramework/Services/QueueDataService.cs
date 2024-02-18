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


    public async Task<IEnumerable<QueueInfo>> GetAll()
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            var queueItems = await dbContext.Set<QueueItem>().ToListAsync();

            var queues = queueItems.Select(x => _queueBuilder.CreateQueueInfo(x.QueueId, x.QueueName, DateTime.Now));

            return queues;
        }
    }

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
            int queueId = queue.QueueInfo.Id;

            QueueItem item = new(queueId, queue.QueueInfo.Name, queue.IsInfinite, queue.CurrentPosition);

            dbContext.Set<QueueItem>().Update(item);

            await dbContext.SaveChangesAsync();

            item = await dbContext.Set<QueueItem>().Include(x => x.Records).FirstAsync(x => x.QueueId == queueId);

            if (item.Records is not null)
            {
                var records = item.Records;
                var queueRecords = queue.Records;

                records.Clear();

                foreach (var queueRecord in queueRecords)
                {
                    QueueRecordItem queueRecordItem = new(queueRecord.Username, queueId, queueRecord.Position);

                    records.Add(queueRecordItem);
                }
            }

            dbContext.Set<QueueItem>().Update(item);

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