namespace SQA.Domain;

public class UserRole
{
    public int Id { get; init; }

    public string Name { get; init; }

    public bool CanManageUsers { get; init; }

    public bool CanManageQueues { get; init; }

    public UserRole(int id, string name, bool canManageUsers, bool canManageQueues)
    {
        Id = id;
        Name = name;
        CanManageUsers = canManageUsers;
        CanManageQueues = canManageQueues;
    }
}