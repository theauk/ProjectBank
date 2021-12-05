using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using ProjectBank.Core.DTOs;
using ProjectBank.Core.IRepositories;

namespace ProjectBank.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> Get(int id)
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<UserDTO>> Get()
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Post(UserCreateDTO tag)
    {
        throw new NotImplementedException();
    }
}