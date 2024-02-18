namespace SQA.EntityFramework.Model;

public class UserItem : DataTableObject
{
    public List<QueueRecordItem>? Records { get; set; }

    public string FullName { get; set; }

    public string Username { get; set; }

    public string PasswordHash { get; set; }

    internal UserItem(string fullName, string username, string passwordHash)
    {
        FullName = fullName;
        Username = username;
        PasswordHash = passwordHash;
    }
}