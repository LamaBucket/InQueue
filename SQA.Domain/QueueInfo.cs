namespace SQA.Domain;

public class QueueInfo
{
    public int Id { get; init; }

    public string Name { get; set; }

    public DateTime Created { get; init; }

    public string OwnerUsername { get; private set; }

    public void PassLeadership(string username)
    {
        OwnerUsername = username;
    }

    internal QueueInfo(int id, string name, DateTime created, string ownerUsername)
    {
        Id = id;
        Name = name;
        Created = created;
        OwnerUsername = ownerUsername;
    }
}