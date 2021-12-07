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

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<TagGroupDTO>> Get(int id)
    {
        var tagGroupDto = await _repository.ReadAsync(id);
        return tagGroupDto.ToActionResult();
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<TagGroupDTO>> Get()
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Post(TagGroupCreateDTO tagGroup)
    {
        Console.WriteLine("im in post");
        var response = await _repository.CreateAsync(tagGroup);

        Console.WriteLine(tagGroup.Name);
        Console.WriteLine(response);
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