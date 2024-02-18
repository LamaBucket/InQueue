
namespace SQA.EntityFramework.Model;

public class QueueItem : DataTableObject
{
    public int QueueId { get; set; }

    public List<QueueRecordItem>? Records { get; set; }

    public string QueueName { get; set; }

    public bool IsInfinite { get; set; }

    public int CurrentPosition { get; set; }

    internal QueueItem(string queueName, bool isInfinite)
    {
        QueueName = queueName;
        IsInfinite = isInfinite;
    }

    internal QueueItem(int queueId, string queueName, bool isInfinite, int currentPosition)
    {
        QueueId = queueId;
        QueueName = queueName;
        IsInfinite = isInfinite;
        CurrentPosition = currentPosition;
    }
}