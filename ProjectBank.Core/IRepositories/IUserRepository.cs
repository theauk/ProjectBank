namespace ProjectBank.Core.IRepositories;

public interface IUserRepository 
{
    Task<Response> CreateAsync(UserCreateDTO user);
    Task<Option<UserDTO>> ReadAsync(int userId);
    Task<IReadOnlyCollection<UserDTO>> ReadAllAsync();
    Task<IReadOnlyCollection<UserDTO>> ReadAllActiveAsync();
}
