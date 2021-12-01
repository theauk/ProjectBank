namespace Infrastructure
{
    public class TagRepository : ITagRepository
    {
        private readonly ProjectBankContext _context;

        public TagRepository(ProjectBankContext context)
        {
            _context = context;
        }

        public async async Task<(Response, TagDTO)> CreateAsync(TagCreateDTO tag)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> DeleteAsync(int tagId)
        {
            throw new NotImplementedException();
        }

        public async Task<(Response, IReadOnlyCollection<TagDTO>)> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<(Response, TagDTO)> ReadAsync(int tagId)
        {
            throw new NotImplementedException();
        }
    }
}
