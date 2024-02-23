using SQA.Domain;

namespace SQA.Domain.Services.Data;

public interface IQueueDataService
{
    Task<IEnumerable<UserQueueInfo>> GetForUser(string username);

    Task<Queue> Get(int id);

    Task Create(string queueName);

    Task Update(Queue queue);

    Task Delete(int id);
}