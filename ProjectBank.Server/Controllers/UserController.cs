namespace ProjectBank.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;

    public UserController(IUserRepository repository) => _repository = repository;

    [Authorize(Roles = Admin)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> Post(UserCreateDTO user)
    {
        var response = await _repository.CreateAsync(user);
        return response.ToActionResult(nameof(Get), response);
    }

    [Authorize(Roles = SuperAdmin)]
    [HttpGet("all")]
    public async Task<IReadOnlyCollection<UserDTO>> GetAll()
    {
        var users = await _repository.ReadAllAsync();
        return users.IsNullOrEmpty() ? new List<UserDTO>().AsReadOnly() : users;
    }

    [Authorize]
    [HttpGet]
    public async Task<IReadOnlyCollection<UserDTO>> Get()
    {
        var users = await _repository.ReadAllByUniversityAsync(User.FindFirstValue(ClaimTypes.Email));
        return users.IsNullOrEmpty() ? new List<UserDTO>().AsReadOnly() : users;
    }

    [Authorize]
    [HttpGet("filter")]
    public async Task<IReadOnlyCollection<UserDTO>> GetActive() 
    {
        var users = await _repository.ReadAllActiveAsync(User.FindFirstValue(ClaimTypes.Email));
        return users.IsNullOrEmpty() ? new List<UserDTO>().AsReadOnly() : users;
    }
    
    [Authorize]
    [HttpGet("roles")]
    public async Task<IReadOnlyCollection<UserDTO>> Get([FromQuery] IList<string> roles)
    {
        var users = await _repository.ReadAllByRoleAsync(User.FindFirstValue(ClaimTypes.Email), roles);
        return users.IsNullOrEmpty() ? new List<UserDTO>().AsReadOnly() : users;
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> Get(int id)
    {
        var user = await _repository.ReadAsync(id);
        return user.ToActionResult();
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
    [HttpGet("{email}")]
    public async Task<ActionResult<UserDTO>> Get(string email)
    {
        var user = await _repository.ReadAsync(email);
        return user.ToActionResult();
    }
}
