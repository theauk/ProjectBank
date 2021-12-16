namespace ProjectBank.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IProjectBankContext _context;

    public UserRepository(IProjectBankContext context) => _context = context;

    public async Task<Response> CreateAsync(UserCreateDTO user)
    {
        var university = await GetUniversity(user.Email);

        if (university == null)
        {
            return Response.BadRequest;
        }

        var entity = new User
        {
            Name = user.Name, 
            Email = user.Email,
            University = university,
            Role = user.Role
        };

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

    public async Task<Option<UserDTO>> ReadAsync(string email)
    {
        return (await _context.Users.FirstOrDefaultAsync(u => u.Email == email))?.ToDTO();
    }

    public async Task<IReadOnlyCollection<UserDTO>> ReadAllByRoleAsync(ISet<string> roles)
    {
        if (roles == null || !roles.Any())
        {
            return await ReadAllAsync();
        }

        return (await _context.Users.Where(u => roles.Select(r => Roles.GetRole(r)).ToHashSet().Contains(u.Role)).ToListAsync()).ToDTO().ToList().AsReadOnly();
    }

    private async Task<University?> GetUniversity(string? email) 
    {
        return string.IsNullOrWhiteSpace(email) ? null : await _context.Universities.FindAsync(email.Split("@")[1]);
    }
}
