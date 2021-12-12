namespace ProjectBank.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UniversityController : ControllerBase
{
    private readonly IUniversityRepository _repository;

    public UniversityController(IUniversityRepository repository)
    {
        _repository = repository;
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UniversityDTO), StatusCodes.Status200OK)]
    [HttpGet("{domain}")]
    public async Task<ActionResult<UniversityDTO?>> Get(string domain)
    {
        var response = await _repository.ReadAsync(domain);
        return response.ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<IActionResult> Post(UniversityCreateDTO university)
    {
        var response = await _repository.CreateAsync(university);
        return CreatedAtAction(nameof(Get), response);
    }

    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{domain}")]
    public async Task<IActionResult> Delete(string domain)
    {
        var response = await _repository.DeleteAsync(domain);
        return response.ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string domain, [FromBody] UniversityUpdateDTO university)
    {
        var response = await _repository.UpdateAsync(domain, university);
        return response.ToActionResult();
    }
}
