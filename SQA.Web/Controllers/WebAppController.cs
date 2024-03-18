using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQA.Domain;
using SQA.Domain.Services.Data;
using SQA.Web.Models;

namespace SQA.Web.Controllers;

public class WebAppController : Controller
{
    private readonly IUserDataService _userDataService;

    private readonly IQueueDataService _queueDataService;

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
    public async Task<IActionResult> Queue(int id)
    {
        var user = await GetUser();
        var queue = await _queueDataService.Get(id);

        bool canManageQueue = queue.QueueInfo.OwnerUsername == user.Username || user.Role.CanManageQueues;

        QueueModel model = new(_username, queue.QueueInfo, canManageQueue);

        return View(model);
    }

    public WebAppController(IUserDataService userDataService, IQueueDataService queueDataService)
    {
        _userDataService = userDataService;
        _queueDataService = queueDataService;
    }
}