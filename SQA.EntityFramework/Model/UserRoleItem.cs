namespace SQA.EntityFramework.Model;

public class UserRoleItem
{
    public IEnumerable<UserItem>? User { get; set; }


    public int Id { get; set; }

    public string Name { get; set; }

    public bool CanManageUsers { get; set; }

    public bool CanManageQueues { get; set; }


    public UserRoleItem(int id, string name, bool canManageUsers, bool canManageQueues)
    {
        Id = id;
        Name = name;
        CanManageUsers = canManageUsers;
        CanManageQueues = canManageQueues;
    }

    public UserRoleItem(string name, bool canManageUsers, bool canManageQueues)
    {
        Name = name;
        CanManageUsers = canManageUsers;
        CanManageQueues = canManageQueues;
    }
}