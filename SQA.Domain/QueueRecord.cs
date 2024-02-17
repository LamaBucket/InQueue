namespace SQA.Domain;

public class QueueRecord : DomainObject
{
    public int Position { get; internal set; }

    public string Username { get; internal set; }

    public QueueRecord(int position, string username)
    {
        Position = position;
        Username = username;
    }
}