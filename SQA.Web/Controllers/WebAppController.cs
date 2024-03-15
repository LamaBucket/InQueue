using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQA.Domain;
using SQA.Domain.Services.Data;
using SQA.Web.Models;

namespace SQA.Web.Controllers;

public class WebAppController : Controller
{
    private readonly IUserDataService _userDataService;

    protected string _username
    {
        get
        {
            return User?.Identity?.Name ?? throw new Exception();
        }
    }

    protected async Task<User> GetUser()
    {
        var user = await _userDataService.Get(_username);

        return user;
    }


    [HttpGet("")]
    public IActionResult HomePage()
    {
        return View();
    }

    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }

    [Authorize]
    [HttpGet("Web")]
    public async Task<IActionResult> WebApp()
    {
        var user = await GetUser();
        
        WebAppModel model = new(user, user.Role.CanManageQueues && user.Role.CanManageUsers);

        return View(model);
    }

    [Authorize]
    [HttpGet("Queue/{id}")]
    public IActionResult Queue(int id)
    {
        return View();
    }

    public WebAppController(IUserDataService userDataService)
    {
        _userDataService = userDataService;
    }
}