using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SQA.Domain;
using SQA.Domain.Services;
using SQA.Domain.Services.Data;
using SQA.EntityFramework.Exceptions;
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

            if (queueItem is null)
                throw new QueueDoesNotExistException(id);

            if (queueItem.Records is null)
                throw new UnableToLoadQueueRecordsException(id);

            var queueRecords = queueItem.Records.Select(x => new QueueRecord(x.Position, x.Username));

            var queueInfo = _queueBuilder.CreateQueueInfo(queueItem.QueueId, queueItem.QueueName, queueItem.OwnerUsername, queueItem.DateCreated);
            var queue = _queueBuilder.CreateQueue(queueInfo, queueItem.CurrentPosition, queueRecords);

            return queue;
        }
    }


    public async Task<IEnumerable<UserQueueInfo>> GetForUser(string username)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            var queueItems = await dbContext.Set<QueueItem>().Include(x => x.Records).Where(x => x.Records != null && x.Records.Any(x => x.Username == username)).ToListAsync();

            List<UserQueueInfo> infos = new();

            foreach (var queueItem in queueItems)
            {
                if (queueItem.Records is null)
                    throw new UnableToLoadQueueRecordsException(queueItem.QueueId);

                int userRealPosition = GetUserRealPosition(username, queueItem.Records);
                int userRelativePosition = CalculateUserRelativePosition(queueItem.CurrentPosition, userRealPosition, queueItem.Records);

                UserQueueInfo info = new(userRelativePosition, queueItem.QueueId, queueItem.QueueName, queueItem.OwnerUsername, queueItem.DateCreated);

                infos.Add(info);
            }

            return infos;
        }
    }

    private int GetUserRealPosition(string username, IEnumerable<QueueRecordItem> records)
    {
        return records.First(x => x.Username == username).Position;
    }

    private int CalculateUserRelativePosition(int currentPosition, int userPosition, IEnumerable<QueueRecordItem> records)
    {
        var recordsArr = records.ToArray();
        int recordLength = recordsArr.Length;

        int userRelativePosition = userPosition - currentPosition;

        if (userPosition < currentPosition)
        {
            userRelativePosition += recordLength;
        }

        return userRelativePosition;
    }


    public async Task Create(string queueName, string ownerUsername)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            var dateCreated = DateTime.Now;

            var queueItem = new QueueItem(queueName, dateCreated, ownerUsername);

            await dbContext.Set<QueueItem>().AddAsync(queueItem);

            await dbContext.SaveChangesAsync();

            await AddUserToQueue(queueItem.QueueId, ownerUsername, 0);
        }
    }

    private async Task AddUserToQueue(int queueId, string username, int position)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {

            var queueItem = await dbContext.Set<QueueItem>().Include(x => x.Records).FirstAsync(x => x.QueueId == queueId);
            var queueRecord = new QueueRecordItem(username, queueItem.QueueId, position);

            queueItem.Records?.Add(queueRecord);

            dbContext.Set<QueueItem>().Update(queueItem);

            await dbContext.SaveChangesAsync();
        }
    }


    public async Task Update(Queue queue)
    {
        if(!queue.Records.Any())
        {
            await Delete(queue.QueueInfo.Id);
        }
        else
        {
            using (var dbContext = _contextFactory.CreateDbContext())
            {
                int queueId = queue.QueueInfo.Id;

                QueueItem item = new(queueId, queue.QueueInfo.Name, queue.CurrentPosition, queue.QueueInfo.Created, queue.QueueInfo.OwnerUsername);

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
    }

    public async Task Delete(int id)
    {
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            var queueItem = await dbContext.Set<QueueItem>().FirstOrDefaultAsync(x => x.QueueId == id);

            if (queueItem is null)
                throw new QueueDoesNotExistException(id);

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