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

    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<TagGroupDTO>> Get()
    {
        
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Post(TagGroupCreateDTO tagGroup)
    {

    }

    [Authorize(Roles = "Admin")] 
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {

    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IACtionResult> Put(int id, [FromBody] TagGroupUpdateDTO tagGroup)
    {
        
    }




}