namespace Infrastructure
{
    public class TagRepository : ITagRepository
    {
        private readonly ProjectBankContext _context;

        public TagRepository(ProjectBankContext context)
        {
            _context = context;
        }

        public Task<(Response, TagDTO)> CreateAsync(TagCreateDTO tag)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(int tagId)
        {
            throw new NotImplementedException();
        }

        public Task<(Response, IReadOnlyCollection<TagDTO>)> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<(Response, TagDTO)> ReadAsync(int tagId)
        {
            throw new NotImplementedException();
        }
    }
}
