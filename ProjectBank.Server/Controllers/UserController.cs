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
    
    public UserController(IUserRepository repository)
    {
        _repository = repository;
    }
    
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
    
    [AllowAnonymous]
    [HttpGet("{role}")]
    public async Task<IReadOnlyCollection<UserDTO>> Get(string role)
    {
        //TODO Implement string role in final final product(out ofscope)
        var supervisorUserDTOs = await _repository.ReadAllAsync(role);
        throw new NotImplementedException(); 
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Post(UserCreateDTO tag)
    {
        throw new NotImplementedException();
    }
}