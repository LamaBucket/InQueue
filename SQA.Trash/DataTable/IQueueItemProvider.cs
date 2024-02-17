using SQA.EntityFramework.Model;

namespace SQA.EntityFramework.Services.DataTable;

public interface IQueueItemProvider : IDataProvider<QueueItem>
{
    Task<QueueItem> Get(int id);
}