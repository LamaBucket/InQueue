using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SQA.Web.Controllers;

[Authorize]
public abstract class AuthenticatedController : Controller
{
    protected string _username
    {
        get
        {
            return ""; //TODO: Get username from cookies;
        }
    }

}