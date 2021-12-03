namespace ProjectBank.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> Get(int id)
    {

    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<UserDTO>> Get()
    {
        
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Post(UserCreateDTO tag)
    {

    }





}