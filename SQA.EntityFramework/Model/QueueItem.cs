
namespace SQA.EntityFramework.Model;

public class QueueItem : DataTableObject
{
    public int QueueId { get; set; }

    public List<QueueRecordItem>? Records { get; set; }

    public string QueueName { get; set; }

    public int CurrentPosition { get; set; }

    public DateTime DateCreated { get; set; }


    internal QueueItem(string queueName, DateTime dateCreated)
    {
        QueueName = queueName;
        DateCreated = dateCreated;
    }

    internal QueueItem(int queueId, string queueName, int currentPosition, DateTime dateCreated)
    {
        QueueId = queueId;
        QueueName = queueName;
        CurrentPosition = currentPosition;
        DateCreated = dateCreated;
    }
}