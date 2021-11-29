namespace Infrastructure
{
    public class UniversityRepository : IUniversityRepository
    {
        private readonly ProjectBankContext _context;

        public UniversityRepository(ProjectBankContext context)
        {
            _context = context;
        }

        public Task<(Response, UniversityDTO)> CreateAsync(UniversityCreateDTO university)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(int universityId)
        {
            throw new NotImplementedException();
        }

        public Task<(Response, IReadOnlyCollection<UniversityDTO>)> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<(Response, UniversityDTO)> ReadAsync(int tagId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(int universityId, UniversityUpdateDTO university)
        {
            throw new NotImplementedException();
        }
    }
}
