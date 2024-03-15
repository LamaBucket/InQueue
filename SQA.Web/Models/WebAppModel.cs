using SQA.Domain;

namespace SQA.Web.Models;

public class WebAppModel
{
    public User User { get; set; }

    public bool HasAccessToLogs { get; init; }

    public WebAppModel(User user, bool hasAccessToLogs)
    {
        User = user;
        HasAccessToLogs = hasAccessToLogs;
    }
}