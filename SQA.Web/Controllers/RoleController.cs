using Microsoft.AspNetCore.Mvc;
using SQA.Domain;
using SQA.Domain.Services.Data;

namespace SQA.Web.Controllers;

[Route("[controller]")]
public class RoleController : AuthenticatedController
{
    private readonly IUserRoleDataService _userRoleDataService;


    [HttpGet]
    public async Task<ActionResult> GetApplicationRoles()
    {
        return Json(await _userRoleDataService.GetAllRoles());
    }

    public RoleController(IUserDataService userDataService, IUserRoleDataService userRoleDataService) : base(userDataService)
    {
        _userRoleDataService = userRoleDataService;
    }
}