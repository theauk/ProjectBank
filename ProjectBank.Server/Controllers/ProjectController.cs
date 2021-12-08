using Blazorise.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using ProjectBank.Core.DTOs;
using ProjectBank.Core.IRepositories;
using ProjectBank.Server.Model;

namespace ProjectBank.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _repository;

    public ProjectController(IProjectRepository repository)
    {
        _repository = repository;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDTO>> Get(int id)
    {
        var projectDto = await _repository.ReadAsync(id);
        return projectDto.ToActionResult();
    }

    [Authorize(Roles = "Admin, Supervisor")]
    [HttpPost]
    public async Task<IActionResult> Post(ProjectCreateDTO project)
    {
        var response = await _repository.CreateAsync(project);
        return response.ToActionResult();
    }

    [Authorize]
    [HttpGet]
    public async Task<IReadOnlyCollection<ProjectDTO>> Get([FromQuery] IEnumerable<int> tagIds)
    {
        IReadOnlyCollection<ProjectDTO> resp;
        if (!tagIds.Any())
            resp = await _repository.ReadAllAsync();
        else
            resp = await _repository.ReadFilteredAsync(tagIds);

        if (resp.IsNullOrEmpty()) return new List<ProjectDTO>();
        return resp;
    }

    [Authorize(Roles = "Admin, Supervisor")] // Vi skal sørge for at supervisors ikke kan slette/opdatere andre supervisors projekter - Skal gøres i Razor/Blazor
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _repository.DeleteAsync(id);
        return response.ToActionResult();
    }

    [Authorize(Roles = "Admin, Supervisor")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ProjectUpdateDTO project)
    {
        var response = await _repository.UpdateAsync(id, project);
        return response.ToActionResult();
    }
}