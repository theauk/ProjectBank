using System.Collections.ObjectModel;

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
            var entity = new User { Name = user.Name, University = _context.Universities.First() };

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

            // // TEMP SUPERVISOR LIST for testing
            // var marco = new UserDTO { Id = 1, Name = "Marco" };
            // var birgit = new UserDTO { Id = 2, Name = "Birgit" };
            // var bjorn = new UserDTO { Id = 3, Name = "Bjørn" };
            // var paolo = new UserDTO { Id = 4, Name = "Paolo" };
            // var rasmus = new UserDTO { Id = 5, Name = "Rasmus" };
            //
            // var usersList = new List<UserDTO>();
            // usersList.Add(marco);
            // usersList.Add(birgit);
            // usersList.Add(bjorn);
            // usersList.Add(paolo);
            // usersList.Add(rasmus);
            // var users = new ReadOnlyCollection<UserDTO>(usersList);

            return users;
            
        }
        
        public async Task<IReadOnlyCollection<UserDTO>> ReadBasedOnRoleAsync(string role)
        {
            //TODO Lave Azure API kald her (Eller hvor det nu giver bedst mening) - Implementér at den sammenligner med databasen, og opretter ny supervisor hvis der mangler
            
            // var users = (await _context.Users.Where(u => u.Role == role ).Select(u => new UserDTO //todo Role skal måske oversættes/omskrives til enum - dette var bare det hurtige som var mest gennemskueligt.
            // {
            //     Id = u.Id,
            //     Name = u.Name
            // }).ToListAsync()).AsReadOnly();
        
            // TEMP SUPERVISOR LIST for testing
            var marco = new UserDTO { Id = 1, Name = "Marco" };
            var birgit = new UserDTO { Id = 2, Name = "Birgit" };
            var bjorn = new UserDTO { Id = 3, Name = "Bjørn" };
            var paolo = new UserDTO { Id = 4, Name = "Paolo" };
            var rasmus = new UserDTO { Id = 5, Name = "Rasmus" };
            
            var usersList = new List<UserDTO>();
            usersList.Add(marco);
            usersList.Add(birgit);
            usersList.Add(bjorn);
            usersList.Add(paolo);
            usersList.Add(rasmus);
            var users = new ReadOnlyCollection<UserDTO>(usersList);

            return users;
            
        }

        public async Task<Option<UserDTO>> ReadAsync(int userId)
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
