namespace Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly ProjectBankContext _context;

        public UserRepository(ProjectBankContext context)
        {
            _context = context;
        }

        public Task<(Response, UserDTO)> CreateAsync(UserCreateDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<(Response, IReadOnlyCollection<UserDTO>)> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<(Response, UserDTO)> ReadAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
