using SQA.Domain;

namespace SQA.Domain.Services;


public interface IQueueDataProvider : IDataProvider<Queue>
{
    Task<Queue> Get(int id);
}