using SQA.Domain;

namespace SQA.Domain.Services.Data;

public interface IQueueDataService
{
    Task<IEnumerable<QueueInfo>> GetAll();

    Task<IEnumerable<QueueInfo>> GetForUser(string username);

    Task<Queue> Get(int id);

    Task Create(string queueName, bool isInfinite);

    Task Update(Queue queue);

    Task Delete(int id);
}