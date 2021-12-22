namespace ProjectBank.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UniversityController : ControllerBase
{
    private readonly IUniversityRepository _repository;

    public UniversityController(IUniversityRepository repository) => _repository = repository;

        [Authorize(Roles = SuperAdmin)]
    [HttpGet("all")]
    public async Task<IReadOnlyCollection<UniversityDTO>> GetAll() 
    {
        var universities = await _repository.ReadAllAsync();
        return universities.IsNullOrEmpty() ? new List<UniversityDTO>().AsReadOnly() : universities;
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UniversityDTO), StatusCodes.Status200OK)]
    [HttpGet("{domain}")]
    public async Task<ActionResult<UniversityDTO>> Get(string domain) => (await _repository.ReadAsync(domain)).ToActionResult();

    [Authorize(Roles = SuperAdmin)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<IActionResult> Post(UniversityCreateDTO university)
    {
        var response = await _repository.CreateAsync(university);
        return response.ToActionResult(nameof(Get), response);
    }

    [Authorize(Roles = SuperAdmin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{domain}")]
    public async Task<IActionResult> Delete(string domain) => (await _repository.DeleteAsync(domain)).ToActionResult();
}