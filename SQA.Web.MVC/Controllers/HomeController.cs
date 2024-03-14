using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SQA.Web.Controllers;

[Route("[controller]")]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult HomePage()
    {
        return View();
    }

    [Authorize]
    [HttpGet("web")]
    public IActionResult WebApp()
    {
        return View();
    }
}