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
public class TagGroupController : ControllerBase
{
    private readonly ITagGroupRepository _repository;

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<TagGroupDTO>> Get(int id)
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<TagGroupDTO>> Get()
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(201)]
    public async Task<IActionResult> Post(TagGroupCreateDTO tagGroup)
    {
        var created = await _repository.CreateAsync(tagGroup);
        return CreatedAtAction(nameof(Get), created);
    }

    [Authorize(Roles = "Admin")] 
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] TagGroupUpdateDTO tagGroup)
    {
        throw new NotImplementedException();
    }
    
    [Authorize(Roles = "Admin")] 
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(int taggroupid, int tagid)
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "Admin, Supervisor")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PostTag(int taggroupid, [FromBody] TagCreateDTO tag)
    {
        throw new NotImplementedException();
    }




}