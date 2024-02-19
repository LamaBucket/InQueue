using System.Resources;

namespace SQA.Domain.Exceptions;

public class UserAlreadyExistsInQueueException : DomainException
{
    public string Username { get; init; }

    public int QueueId { get; init; }

    public UserAlreadyExistsInQueueException(string username, int queueId)
    {
        Username = username;
        QueueId = queueId;
    }
}