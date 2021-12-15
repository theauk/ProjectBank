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
    [HttpGet("roles/{role}")]
    public async Task<IReadOnlyCollection<UserDTO>> Get(string role = "all") => await _repository.ReadAllByRoleAsync(role);
}