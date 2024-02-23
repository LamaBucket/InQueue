namespace SQA.Domain.Services;

public interface IQueueBuilder
{
    QueueInfo CreateQueueInfo(int id, string name, string ownerUsername, DateTime dateCreated);

    Queue CreateQueue(QueueInfo info, int currentPosition, IEnumerable<QueueRecord> records);
}