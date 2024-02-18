using Microsoft.AspNetCore.Mvc;
using SQA.Domain.Services.Data;

namespace SQA.Web.Controllers;

[Route("logon")]
public class LogonController : Controller
{
    private readonly IUserDataService _userDataService;

    [HttpGet]
    public async Task<ActionResult> Login(string username, string password)
    {
        var user = await _userDataService.Get(username);

        bool passwordValid = user.ValidatePassword(password);

        if (passwordValid)
        {
            //TODO: Implement authorization logic;

            return Ok();
        }

        return Unauthorized();
    }

    public LogonController(IUserDataService userDataService)
    {
        _userDataService = userDataService;
    }
}