using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQA.Domain.Services.Data;

namespace SQA.Web.Controllers;

[Route("Authenticate")]
public class LogonController : Controller
{
    private readonly IUserDataService _userDataService;

    [HttpPost]
    public async Task<ActionResult> Login(string username, string password)
    {
        var user = await _userDataService.Get(username);

        bool passwordValid = user.ValidatePassword(password);

        if (passwordValid)
        {
            var claims = new List<Claim>() { new Claim(ClaimTypes.Name, username) };
            var identity = new ClaimsIdentity(claims, "Cookies");
            ClaimsPrincipal principal = new(identity);

            return SignIn(principal);
        }

        return BadRequest();
    }

    [HttpPost("/Guest")]
    public async Task<ActionResult> LoginGuest()
    {
        var guestUsername = Program.GuestUsername;

        var guestUser = await _userDataService.Get(guestUsername);

        var claims = new List<Claim>() { new Claim(ClaimTypes.Name, guestUser.Username) };
        var identity = new ClaimsIdentity(claims, "Cookies");
        ClaimsPrincipal principal = new(identity);

        return SignIn(principal);
    }


    [Authorize]
    [HttpDelete]
    public ActionResult Logout()
    {
        return SignOut();
    }

    public LogonController(IUserDataService userDataService)
    {
        _userDataService = userDataService;
    }
}