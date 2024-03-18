using SQA.Domain;

namespace SQA.Web.Models;

public class QueueModel
{
    public string Username { get; init; }

    public QueueInfo Info { get; init; }

    public bool CanManageQueue { get; init; }

    public QueueModel(string username, QueueInfo info, bool canManageQueue)
    {
        Username = username;
        Info = info;
        CanManageQueue = canManageQueue;
    }
}