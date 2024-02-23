using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQA.Domain;
using SQA.Domain.Services.Data;

namespace SQA.Web.Controllers;

[Authorize]
public abstract class AuthenticatedController : Controller
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

    public AuthenticatedController(IUserDataService userDataService)
    {
        _userDataService = userDataService;
    }
}