namespace ProjectBank.Core.IRepositories;

public interface IUserRepository 
{
    Task<Response> CreateAsync(UserCreateDTO user);
    Task<Option<UserDTO>> ReadAsync(int userId);
    Task<Option<UserDTO>> ReadAsync(string email);
    Task<IReadOnlyCollection<UserDTO>> ReadAllAsync();
    Task<IReadOnlyCollection<UserDTO>> ReadAllActiveAsync();
    Task<IReadOnlyCollection<UserDTO>> ReadAllByRoleAsync(string role);
}
