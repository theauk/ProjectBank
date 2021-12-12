namespace ProjectBank.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;

    public UserController(IUserRepository repository)
    {
        _repository = repository;
    }

    [Authorize]
    [HttpGet("roles/{role}")]
    public async Task<IReadOnlyCollection<UserDTO>> Get(string role = "all")
    {
        if (role == "all")
        {
            var supervisorUserDTOs = await _repository.ReadAllAsync();
            return supervisorUserDTOs;
        }
        else
        {
            var supervisorUserDTOs = await _repository.ReadAllAsync();
            // var supervisorUserDTOs = await _repository.ReadBasedOnRoleAsync(role); // ToDo Need implementation in User database/repository  via Azure API Call
            return supervisorUserDTOs;
        }
    }
}