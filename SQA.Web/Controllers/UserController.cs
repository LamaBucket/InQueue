using Microsoft.AspNetCore.Mvc;
using SQA.Domain.Services.Data;

namespace SQA.Web.Controllers;


public class UserController : Controller
{
    private readonly IUserDataService _userDataService;

    private string _username
    {
        get
        {
            return ""; //TODO: Get username from cookies;
        }
    }


    [HttpGet]
    public async Task<ActionResult> GetUserInfo()
    {
        var user = await _userDataService.Get(_username);

        return Json(user);
    }

    [HttpPut("Name")]
    public async Task<ActionResult> Rename(string fullName)
    {
        var user = await _userDataService.Get(_username);

        user.FullName = fullName;

        await _userDataService.Update(user);

        return Ok();
    }

    [HttpPut("Password")]
    public async Task<ActionResult> ChangePassword(string oldPassword, string newPassword)
    {
        var user = await _userDataService.Get(_username);

        user.UpdatePassword(oldPassword, newPassword);

        await _userDataService.Update(user);

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUser()
    {
        await _userDataService.Delete(_username);

        return Ok();
    }

    public UserController(IUserDataService userDataService)
    {
        _userDataService = userDataService;
    }
}