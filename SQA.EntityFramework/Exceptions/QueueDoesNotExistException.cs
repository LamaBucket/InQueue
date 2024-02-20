using SQA.Domain.Exceptions;

namespace SQA.EntityFramework.Exceptions;

public class QueueDoesNotExistException : DomainException
{
    public int QueueId { get; init; }

    public QueueDoesNotExistException(int queueId)
    {
        QueueId = queueId;
    }
}