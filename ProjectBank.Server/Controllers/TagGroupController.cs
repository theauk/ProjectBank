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
public class TagGroupController : ControllerBase
{
    private readonly ITagGroupRepository _repository;

    public TagGroupController(ITagGroupRepository repository)
    {
        _repository = repository;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<TagGroupDTO>> Get(int id)
    {
        var tagGroupDto = await _repository.ReadAsync(id);
        return tagGroupDto.ToActionResult();
    }

    [Authorize]
    [HttpGet]
    public async Task<IReadOnlyCollection<TagGroupDTO>> Get()
    {
        var tagGroupDtOs = await _repository.ReadAllAsync();
        return tagGroupDtOs;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Post(TagGroupCreateDTO tagGroup)
    {
        var response = await _repository.CreateAsync(tagGroup);
        return response.ToActionResult();
    }

    [Authorize(Roles = "Admin")] 
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _repository.DeleteAsync(id);
        return response.ToActionResult();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] TagGroupUpdateDTO tagGroup)
    {
        var response = await _repository.UpdateAsync(id, tagGroup);
        return response.ToActionResult();
    }
}