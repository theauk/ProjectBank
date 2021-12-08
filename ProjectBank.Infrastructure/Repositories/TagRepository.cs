namespace ProjectBank.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly IProjectBankContext _context;

        public TagRepository(IProjectBankContext context)
        {
            _context = context;
        }

        public async Task<Response> CreateAsync(TagCreateDTO tag)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> DeleteAsync(int tagId)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<TagDTO>> ReadAllAsync()
        {
            var tags = (await _context.Tags.Select(t => new TagDTO
            {
                Id = t.Id,
                Value = t.Value
            }).ToListAsync()).AsReadOnly();

            return tags;
        }

        public async Task<Option<TagDTO?>> ReadAsync(int tagId)
        {
            var entity = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagId);

            return entity == null ? null :  new TagDTO
            {
                Id = entity.Id,
                Value = entity.Value
            };
        }
    }
}
