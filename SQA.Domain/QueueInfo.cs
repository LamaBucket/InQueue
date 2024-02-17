namespace SQA.Domain;

public class QueueInfo
{
    public int Id { get; init; }

    public string Name { get; protected set; }

    public DateTime Created { get; init; }

    public void UpdateName(string name)
    {
        Name = name;
    }

    internal QueueInfo(int id, string name, DateTime created)
    {
        Id = id;
        Name = name;
        Created = created;
    }
}