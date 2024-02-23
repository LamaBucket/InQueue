
namespace SQA.Domain;

public class UserQueueInfo : QueueInfo
{
    public int UserPosition { get; set; }

    public UserQueueInfo(int userPosition, int id, string name, DateTime created) : base(id, name, created)
    {
        UserPosition = userPosition;
    }
}