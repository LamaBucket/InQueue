namespace SQA.Domain;

public class QueueInfo
{
    public int Id { get; init; }

    public string Name { get; set; }

    public DateTime Created { get; init; }

    internal QueueInfo(int id, string name, DateTime created)
    {
        Id = id;
        Name = name;
        Created = created;
    }
}