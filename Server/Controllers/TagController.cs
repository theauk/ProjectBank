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
public class TagController : ControllerBase
{
    private readonly ITagRepository _repository;

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<TagDTO>> Get(int id)
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<TagDTO>> Get()
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "Admin, Supervisor")]
    [HttpPost]
    public async Task<IActionResult> Post(TagCreateDTO tag)
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
}