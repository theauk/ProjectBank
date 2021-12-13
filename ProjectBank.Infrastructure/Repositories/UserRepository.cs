using System.Collections.ObjectModel;

namespace ProjectBank.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IProjectBankContext _context;

    public UserRepository(IProjectBankContext context) => _context = context;

    public async Task<Response> CreateAsync(UserCreateDTO user)
    {
        var entity = new User {Name = user.Name, Email = user.Name, University = _context.Universities.First()};

        _context.Users.Add(entity);

        await _context.SaveChangesAsync();

        return Response.Created;
    }

    public async Task<IReadOnlyCollection<UserDTO>> ReadAllAsync() => (await _context.Users
        .ToListAsync()).ToDTO().ToList().AsReadOnly();
        
    public async Task<IReadOnlyCollection<UserDTO>> ReadAllActiveAsync() => (await _context.Users
        .Where(u => u.Projects.Count > 0)
        .ToListAsync()).ToDTO().ToList().AsReadOnly();

    public async Task<Option<UserDTO>> ReadAsync(int userId) => 
        (await _context.Users.FindAsync(userId))?.ToDTO();
}
