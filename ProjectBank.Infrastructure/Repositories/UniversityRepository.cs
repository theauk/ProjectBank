namespace ProjectBank.Infrastructure.Repositories
{
    public class UniversityRepository : IUniversityRepository
    {
        private readonly IProjectBankContext _context;

        public UniversityRepository(IProjectBankContext context)
        {
            _context = context;
        }

        public async Task<Response> CreateAsync(UniversityCreateDTO university)
        {
            var entity = new University { DomainName = university.DomainName };

            _context.Universities.Add(entity);

            await _context.SaveChangesAsync();

            return Response.Created;
        }

        public async Task<Response> UpdateAsync(int universityId, UniversityUpdateDTO university)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> DeleteAsync(int universityId)
        {
            var entity = await _context.Universities.FirstOrDefaultAsync(u => u.Id == universityId);

            if (entity == null)
            {
                return Response.NotFound;
            }

            _context.Universities.Remove(entity);
            await _context.SaveChangesAsync();

            return Response.Deleted;
        }

        public async Task<Option<UniversityDTO?>> ReadAsync(int universityId)
        {
            var entity = await _context.Universities.FirstOrDefaultAsync(u => u.Id == universityId);

            return entity == null ? null  : new UniversityDTO 
            {
                Id = entity.Id,
                DomainName = entity.DomainName,
                UserIds = entity.Users.Select(u => u.Id).ToHashSet(),
                ProjectIds = entity.Projects.Select(p => p.Id).ToHashSet(),
                TagGroupIds = entity.TagGroups.Select(tg => tg.Id).ToHashSet()
            };
        }
    }
}
