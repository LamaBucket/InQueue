namespace SQA.Domain.Services;

public interface IQueueBuilder
{
    QueueInfo CreateQueueInfo(int id, string name, DateTime dateCreated);

    Queue CreateQueue(QueueInfo info, int currentPosition, bool isInfinite, IEnumerable<QueueRecord> records);
}