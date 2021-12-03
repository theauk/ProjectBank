namespace ProjectBank.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ProjectController : ControllerBase
{
    // Skal kun kaldes hvis brugeren har forbindelsetil/affiliation med universitetet
    private readonly IProjectRepository _repository;

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDTO>> Get(int id)
    {

    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<ProjectDTO>> Get()
    {
        
    }

    [Authorize(Roles = "SuperAdmin")] //SuperAdmin for at vise at en alm, admin ikke skal kunne gøre disse ting
    [HttpPost]
    public async Task<IActionResult> Post(ProjectCreateDTO project)
    {

    }

    [Authorize(Roles = "SuperAdmin")] 
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {

    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpPut("{id}")]
    public async Task<IACtionResult> Put(int id, [FromBody] ProjectUpdateDTO project)
    {
        
    }




}