namespace ProjectBank.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IProjectBankContext _context;

        public UserRepository(IProjectBankContext context)
        {
            _context = context;
        }

        public async Task<Response> CreateAsync(UserCreateDTO user)
        {
            var entity = new User { Name = user.Name };

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
        
        public async Task<IReadOnlyCollection<UserDTO>> ReadAllAsync(string role)
        {
            //TODO Lave Azure API kald her
            // var users = (await _context.Users.Where(u => u.Role == role ).Select(u => new UserDTO
            // {
            //     Id = u.Id,
            //     Name = u.Name
            // }).ToListAsync()).AsReadOnly();
            
            var marco = new User { Id = 1, Name = "Marco" };
            var birgit = new User { Id = 2, Name = "Birgit" };
            var bjorn = new User { Id = 3, Name = "Bjørn" };
            var paolo = new User { Id = 4, Name = "Paolo" };
            var rasmus = new User { Id = 5, Name = "Rasmus" };
            
            // var users = new List

            return users;
        }

        public async Task<Option<UserDTO?>> ReadAsync(int userId)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return entity == null ? null : new UserDTO
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
