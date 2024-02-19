using System.Resources;

namespace SQA.Domain.Exceptions;

public class RecordDoesNotExist : DomainException
{
    public int QueueId { get; init; }

    public QueueRecord Record { get; init; }

    public RecordDoesNotExist(int queueId, QueueRecord record)
    {
        QueueId = queueId;
        Record = record;
    }
}