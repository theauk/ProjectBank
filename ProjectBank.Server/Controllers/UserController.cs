namespace ProjectBank.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;

    public UserController(IUserRepository repository) => _repository = repository;

    [Authorize]
    [HttpGet("filter")]
    public async Task<IReadOnlyCollection<UserDTO>> Get() => await _repository.ReadAllActiveAsync();
    
    [Authorize]
    [HttpGet("roles")]
    public async Task<IReadOnlyCollection<UserDTO>> Get([FromQuery] IList<string> roles) => await _repository.ReadAllByRoleAsync(roles.ToHashSet());
}