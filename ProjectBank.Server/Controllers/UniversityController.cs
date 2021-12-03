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
public class UniversityController : ControllerBase
{
    // Skal kun kaldes hvis brugeren har forbindelsetil/affiliation med universitetet
    private readonly IProjectRepository _repository;

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDTO>> Get(int id)
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<ProjectDTO>> Get()
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "SuperAdmin")] //SuperAdmin for at vise at en alm, admin ikke skal kunne gøre disse ting
    [HttpPost]
    public async Task<IActionResult> Post(ProjectCreateDTO project)
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "SuperAdmin")] 
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ProjectUpdateDTO project)
    {
        throw new NotImplementedException();
    }




}