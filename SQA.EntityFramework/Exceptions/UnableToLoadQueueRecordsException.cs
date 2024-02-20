using SQA.Domain.Exceptions;

namespace SQA.EntityFramework.Exceptions;

public class UnableToLoadQueueRecordsException : DomainException
{
    public int QueueId { get; init; }

    public UnableToLoadQueueRecordsException(int queueId)
    {
        QueueId = queueId;
    }
}