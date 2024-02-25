using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SQA.Domain;
using SQA.Domain.Services.Data;

namespace SQA.Web.Controllers;

[Authorize]
public abstract class AuthenticatedHub : Hub
{
    private readonly IUserDataService _userDataService;

    protected string _username
    {
        get
        {
            return Context.User?.Identity?.Name ?? throw new Exception();
        }
    }

    protected async Task<User> GetUser()
    {
        var user = await _userDataService.Get(_username);

        return user;
    }

    public AuthenticatedHub(IUserDataService userDataService)
    {
        _userDataService = userDataService;
    }
}