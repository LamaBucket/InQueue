using System.Resources;

namespace SQA.Domain.Exceptions;

public class EmptyQueueException : DomainException
{
    public int QueueId { get; init; }

    public EmptyQueueException(int queueId)
    {
        QueueId = queueId;
    }
}