namespace Infrastructure
{
    public class UniversityRepository : IUniversityRepository
    {
        private readonly ProjectBankContext _context;

        public UniversityRepository(ProjectBankContext context)
        {
            _context = context;
        }

        public async Task<(Response, UniversityDTO)> CreateAsync(UniversityCreateDTO university)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> DeleteAsync(int universityId)
        {
            throw new NotImplementedException();
        }

        public async Task<(Response, IReadOnlyCollection<UniversityDTO>)> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<(Response, UniversityDTO)> ReadAsync(int tagId)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> UpdateAsync(int universityId, UniversityUpdateDTO university)
        {
            throw new NotImplementedException();
        }
    }
}
