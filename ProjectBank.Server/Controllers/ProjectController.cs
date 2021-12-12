using Blazorise.Extensions;

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

    [Authorize]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDTO?>> Get(int id)
    {
        var projectDto = await _repository.ReadAsync(id);
        return projectDto.ToActionResult();
    }

    [Authorize(Roles = "Admin, Supervisor")]
    [HttpPost]
    public async Task<IActionResult> Post(ProjectCreateDTO project)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var name = User.FindFirstValue("name");
        var response = await _repository.CreateAsync(project, email, name);
        return response.ToActionResult();
    }

    [Authorize]
    [HttpGet]
    public async Task<IReadOnlyCollection<ProjectDTO>> Get([FromQuery] IList<int> tagIds, [FromQuery] IList<int> supervisorIds)
    {
        IReadOnlyCollection<ProjectDTO> resp;
        if (!tagIds.Any() && !supervisorIds.Any())
            resp = await _repository.ReadAllAsync();
        else
            resp = await _repository.ReadFilteredAsync(tagIds, supervisorIds);

        return resp.IsNullOrEmpty() ? new List<ProjectDTO>().AsReadOnly() : resp;
    }

    [Authorize(Roles = "Admin, Supervisor")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<IActionResult> Post(ProjectCreateDTO project)
    {
        var response = await _repository.CreateAsync(project);
        return CreatedAtAction(nameof(Get), response);
    }

    [Authorize(Roles = "Admin, Supervisor")] //ToDo  Vi skal sørge for at supervisors ikke kan slette/opdatere andre supervisors projekter - Skal gøres i Razor/Blazor
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _repository.DeleteAsync(id);
        return response.ToActionResult();
    }

    [Authorize(Roles = "Admin, Supervisor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ProjectUpdateDTO project)
    {
        var response = await _repository.UpdateAsync(id, project);
        return response.ToActionResult();
    }
}