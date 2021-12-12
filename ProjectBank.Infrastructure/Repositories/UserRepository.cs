using System.Collections.ObjectModel;

namespace ProjectBank.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IProjectBankContext _context;

    public UserRepository(IProjectBankContext context)
    {
        _context = context;
    }

    public async Task<Response> CreateAsync(UserCreateDTO user)
    {
        var entity = new User {Name = user.Name, Email = user.Name, University = _context.Universities.First()};

        _context.Users.Add(entity);

        await _context.SaveChangesAsync();

        return Response.Created;
    }

    public async Task<IReadOnlyCollection<UserDTO>> ReadAllAsync()
    {
        var users = (await _context.Users.Select(u => new UserDTO
        {
            Id = u.Id,
            Name = u.Name
        }).ToListAsync()).AsReadOnly();

            return users;
        }
        
        public async Task<IReadOnlyCollection<UserDTO>> ReadAllActiveAsync()
        {
            return (await _context.Users.Where(u => u.Projects.Count > 0)
                .Select(u => u.ToDTO()).ToListAsync()).AsReadOnly();
        }

    public async Task<Option<UserDTO>> ReadAsync(int userId)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        return entity == null
            ? null
            : new UserDTO
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
