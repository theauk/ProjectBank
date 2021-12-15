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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(TagGroupDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<TagGroupDTO?>> Get(int id)
    {
        var response = await _repository.ReadAsync(id);
        return response.ToActionResult();
    }

    [Authorize]
    [HttpGet]
    public async Task<IReadOnlyCollection<TagGroupDTO>> Get()
    {
        return await _repository.ReadAllAsync();
    }

    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<IActionResult> Post(TagGroupCreateDTO tagGroup)
    {
        var response = await _repository.CreateAsync(tagGroup);
        return CreatedAtAction(nameof(Get), response);
    }

    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _repository.DeleteAsync(id);
        return response.ToActionResult();
    }

    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] TagGroupUpdateDTO tagGroup)
    {
        var response = await _repository.UpdateAsync(id, tagGroup);
        return response.ToActionResult();
    }
}
