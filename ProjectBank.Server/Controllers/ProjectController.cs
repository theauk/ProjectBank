using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _repository;

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDTO>> Get(int id)
    {
        var resp = _repository.ReadAsync(id);
        if (resp.Item1 == ProjectBank.Core.Response.Success)
            return resp.Item2;
        else
            return null;
    }

    [Authorize(Roles = "Admin, Supervisor")]
    [HttpPost]
    public async Task<IActionResult> Post(ProjectCreateDTO project)
    {
        var resp = await _repository.CreateAsync(project);
        
        switch (resp.Item1)
        {
            case ProjectBank.Core.Response.Success:
                return Created($"index/{resp.Item2}");
            default:
                return Problem();
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IReadOnlyCollection<ProjectDTO>> Get([FromQuery] IEnumerable<int> tagIds)
    {
        (ProjectBank.Core.Response, IReadOnlyCollection<ProjectDTO>) resp;
        if (tagIds == null || !tagIds.Any())
            resp = await _repository.ReadAllAsync();
        else
            resp = await _repository.ReadFilteredAsync(tagIds);
        
        if (resp.Item1 == ProjectBank.Core.Response.Success)
            return resp.Item2;
        else
            return new List<ProjectDTO>();
    }

    [Authorize(Roles = "Admin, Supervisor")] // Vi skal sørge for at supervisors ikke kan slette/opdatere andre supervisors projekter - Skal gøres i Razor/Blazor
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resp = await _repository.DeleteAsync(id);

        switch (resp.Item1)
        {
            case ProjectBank.Core.Response.Success:
                return Accepted();
            default:
                return Problem();
        }
    }

    [Authorize(Roles = "Admin, Supervisor")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ProjectUpdateDTO project)
    {
        var resp = await _repository.UpdateAsync(id, project);

        switch (resp.Item1)
        {
            case ProjectBank.Core.Response.Success:
                return Accepted();
            default:
                return Problem();
        }
    }
}