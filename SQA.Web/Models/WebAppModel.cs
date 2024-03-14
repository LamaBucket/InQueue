using SQA.Domain;

namespace SQA.Web.Models;

public class WebAppModel
{
    public string UserFullName { get; init; }

    public bool HasAccessToLogs { get; init; }

    public WebAppModel(string userFullName, bool hasAccessToLogs)
    {
        UserFullName = userFullName;
        HasAccessToLogs = hasAccessToLogs;
    }
}