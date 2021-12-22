namespace ProjectBank.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _repository;

    public ProjectController(IProjectRepository repository) => _repository = repository;

    [Authorize(Roles = $"{Admin}, {Supervisor}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> Post(ProjectCreateDTO project)
    {
        var response = await _repository.CreateAsync(project, User.FindFirstValue(ClaimTypes.Email));
        return response.ToActionResult(nameof(Get), response);
    }

    [Authorize(Roles = SuperAdmin)]
    [HttpGet("all")]
    public async Task<IReadOnlyCollection<ProjectDTO>> GetAll()
    {
        var projects = await _repository.ReadAllAsync();
        return projects.IsNullOrEmpty() ? new List<ProjectDTO>().AsReadOnly() : projects;
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDTO>> Get(int id)
    {
        var project = await _repository.ReadAsync(id);
        return project.ToActionResult();
    }

    [Authorize]
    [HttpGet]
    public async Task<IReadOnlyCollection<ProjectDTO>> Get([FromQuery] IList<int> tagIds, [FromQuery] IList<int> supervisorIds)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email); 
        IReadOnlyCollection<ProjectDTO> resp;
        if (!tagIds.Any() && !supervisorIds.Any())
            resp = await _repository.ReadAllByUniversityAsync(userEmail);
        else
            resp = await _repository.ReadFilteredAsync(userEmail, tagIds, supervisorIds);

        return resp.IsNullOrEmpty() ? new List<ProjectDTO>().AsReadOnly() : resp;
    }

    [Authorize(Roles = $"{Admin}, {Supervisor}")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _repository.DeleteAsync(id);
        return response.ToActionResult();
    }

    [Authorize(Roles = $"{Admin}, {Supervisor}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ProjectUpdateDTO project)
    {
        var response = await _repository.UpdateAsync(id, project);
        return response.ToActionResult();
    }
}
