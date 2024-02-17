namespace SQA.EntityFramework.Model;

public class QueueRecordItem : DataTableObject
{
    public QueueItem? Queue { get; init; }

    public int QueueId { get; init; }

    public UserItem? User { get; init; }

    public string Username { get; set; }

    public int Position { get; init; }

    internal QueueRecordItem(string username, int queueId, int position)
    {
        QueueId = queueId;
        Position = position;
        Username = username;
    }
}