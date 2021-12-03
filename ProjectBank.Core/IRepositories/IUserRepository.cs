using ProjectBank.Core.DTOs;

namespace ProjectBank.Core.IRepositories;

public interface IUserRepository 
{
    Task<(Response, UserDTO)> CreateAsync(UserCreateDTO user);
    Task<(Response, UserDTO)> ReadAsync(int userId);
    Task<(Response, IReadOnlyCollection<UserDTO>)> ReadAllAsync();
}