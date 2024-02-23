using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQA.Domain.Services.Data;

namespace SQA.Web.Controllers;

public class UserController : AuthenticatedController
{
    private readonly IUserDataService _userDataService;

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

    [HttpPost]
    public async Task<ActionResult> CreateUser(string username, string fullName, string password, int roleId)
    {
        if (await CanManageUsers())
        {
            await _userDataService.Create(username, fullName, password, roleId);

            return Ok();
        }

        return Forbid();
    }

    private async Task<bool> CanManageUsers()
    {
        var user = await GetUser();

        return user.Role.CanManageUsers;
    }

    public UserController(IUserDataService userDataService) : base(userDataService)
    {
        _userDataService = userDataService;
    }
}