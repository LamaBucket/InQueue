
using SQA.Domain;

namespace SQA.EntityFramework.Model;

public class QueueItem : DataTableObject
{
    public int QueueId { get; set; }

    public List<QueueRecordItem>? Records { get; set; }

    public string QueueName { get; set; }

    public int CurrentPosition { get; set; }

    public DateTime DateCreated { get; set; }

    public UserItem? User { get; set; }

    public string OwnerUsername { get; set; }


    internal QueueItem(string queueName, DateTime dateCreated, string ownerUsername)
    {
        QueueName = queueName;
        DateCreated = dateCreated;
        OwnerUsername = ownerUsername;
    }

    internal QueueItem(int queueId, string queueName, int currentPosition, DateTime dateCreated, string ownerUsername)
    {
        QueueId = queueId;
        QueueName = queueName;
        CurrentPosition = currentPosition;
        DateCreated = dateCreated;
        OwnerUsername = ownerUsername;
    }
}