
namespace SQA.Domain.Services;

public class QueueBuilder : IQueueBuilder
{
    public Queue CreateQueue(QueueInfo info, int currentPosition, bool isInfinite, IEnumerable<QueueRecord> records)
    {
        return new Queue(info, currentPosition, isInfinite, records);
    }

    public QueueInfo CreateQueueInfo(int id, string name, DateTime dateCreated)
    {
        return new QueueInfo(id, name, dateCreated);
    }
}