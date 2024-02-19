
namespace SQA.EntityFramework.Model;

public class QueueItem : DataTableObject
{
    public int QueueId { get; set; }

    public List<QueueRecordItem>? Records { get; set; }

    public string QueueName { get; set; }

    public bool IsInfinite { get; set; }

    public int CurrentPosition { get; set; }

    public DateTime DateCreated { get; set; }


    internal QueueItem(string queueName, bool isInfinite, DateTime dateCreated)
    {
        QueueName = queueName;
        IsInfinite = isInfinite;
        DateCreated = dateCreated;
    }

    internal QueueItem(int queueId, string queueName, bool isInfinite, int currentPosition, DateTime dateCreated)
    {
        QueueId = queueId;
        QueueName = queueName;
        IsInfinite = isInfinite;
        CurrentPosition = currentPosition;
        DateCreated = dateCreated;
    }
}