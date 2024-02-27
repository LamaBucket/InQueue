using Microsoft.AspNetCore.Mvc;
using SQA.Domain.Services.Data;

namespace SQA.Web.Controllers;

public class LoggingController : AuthenticatedController
{
    [HttpGet]
    public async Task<ActionResult> GetCurrentLog()
    {
        if (await UserHasAccessToLog())
        {
            var content = new FileStream("output.log", FileMode.Open, FileAccess.Read);

            return File(content, "APPLICATION/octet-stream");
        }

        return Forbid();
    }

    private async Task<bool> UserHasAccessToLog()
    {
        var user = await GetUser();

        return user.Role.CanManageQueues & user.Role.CanManageUsers;
    }

    public LoggingController(IUserDataService userDataService) : base(userDataService)
    {
    }
}