namespace Core;

public interface IUserRepository 
{
    Task<(Response, UserDTO)> CreateAsync(UserCreateDTO user);
    Task<(Response, UserDTO)> ReadAsync(int userId);
    Task<(Response, IReadOnlyCollection<UserDTO>)> ReadAllAsync();
}