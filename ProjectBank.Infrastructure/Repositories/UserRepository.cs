namespace ProjectBank.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IProjectBankContext _context;

    public UserRepository(IProjectBankContext context) => _context = context;

    public async Task<Response> CreateAsync(UserCreateDTO user)
    {
        var university = await GetUniversityAsync(user.Email);

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

    public async Task<IReadOnlyCollection<UserDTO>> ReadAllActiveAsync(string email)
    {
        var uni = await GetUniversityAsync(email);
        return (await _context.Users.Where(u => u.Projects.Count > 0).Where(u => u.University.DomainName == uni.DomainName).ToListAsync()).ToDTO().ToList().AsReadOnly();
    } 

    public async Task<Option<UserDTO>> ReadAsync(int userId) =>
        (await _context.Users.FindAsync(userId))?.ToDTO();

    public async Task<Option<UserDTO>> ReadAsync(string email)
    {
        return (await _context.Users.FirstOrDefaultAsync(u => u.Email == email))?.ToDTO();
    }

    public async Task<IReadOnlyCollection<UserDTO>> ReadAllByRoleAsync(string email, IList<string> roles)
    {
        if (roles == null || !roles.Any())
        {
            return await ReadAllByUniversityAsync(email);
        }

        return (await ReadAllByUniversityAsync(email)).Where(u => roles.Select(r => Roles.GetRole(r)).ToHashSet().Contains(u.Role)).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<UserDTO>> ReadAllByUniversityAsync(string email)
    {
        var university = await GetUniversityAsync(email);
        return university == null ? new List<UserDTO>() : university.Users.ToDTO().ToList().AsReadOnly();
    }

    private async Task<University?> GetUniversityAsync(string email)
    {
        var domain = email.Split("@")[1];
        return string.IsNullOrWhiteSpace(email) ? null : await _context.Universities.Include(u => u.Users).Include(u => u.Projects).Include(u => u.TagGroups).FirstOrDefaultAsync(u => u.DomainName == domain);
    }
}
