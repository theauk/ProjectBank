namespace Infrastructure
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
            throw new NotImplementedException();
        }

        public async Task<(Response, IReadOnlyCollection<UserDTO>)> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<(Response, UserDTO)> ReadAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
