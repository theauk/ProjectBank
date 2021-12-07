namespace ProjectBank.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IProjectBankContext _context;

        public UserRepository(IProjectBankContext context)
        {
            _context = context;
        }

        public async Task<(Response, UserDTO)> CreateAsync(UserCreateDTO user)
        {
            var entity = new User { Name = user.Name };

            _context.Users.Add(entity);

            await _context.SaveChangesAsync();

            return (Response.Created, new UserDTO {
                Id = entity.Id,
                Name = entity.Name
            });
        }

        public async Task<(Response, IReadOnlyCollection<UserDTO>)> ReadAllAsync()
        {
            var users = (await _context.Users.Select(u => new UserDTO
            {
                Id = u.Id,
                Name = u.Name
            }).ToListAsync()).AsReadOnly();

            return (Response.Success, users);
        }

        public async Task<(Response, UserDTO?)> ReadAsync(int userId)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (entity == null)
            {
                return (Response.NotFound, null);
            }

            return (Response.Success, new UserDTO
            {
                Id = entity.Id,
                Name = entity.Name
            });
        }
    }
}
