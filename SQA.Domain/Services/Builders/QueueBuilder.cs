
namespace SQA.Domain.Services;

public class QueueBuilder : IQueueBuilder
{
    public Queue CreateQueue(QueueInfo info, int currentPosition, IEnumerable<QueueRecord> records)
    {
        return new Queue(info, currentPosition, records);
    }

    public QueueInfo CreateQueueInfo(int id, string name, string ownerUsername, DateTime dateCreated)
    {
        return new QueueInfo(id, name, dateCreated, ownerUsername);
    }
}