namespace Infrastructure
{
    public class TagGroupRepository : ITagGroupRepository
    {
        private readonly ProjectBankContext _context;

        public TagGroupRepository(ProjectBankContext context)
        {
            _context = context;
        }

        public async Task<(Response, TagGroupDTO)> CreateAsync(TagGroupCreateDTO tagGroup)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> DeleteAsync(int tagGroupId)
        {
            throw new NotImplementedException();
        }

        public async Task<(Response, IReadOnlyCollection<TagGroupDTO>)> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<(Response, TagGroupDTO)> ReadAsync(int tagGroupId)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> UpdateAsync(int tagGroupId, TagGroupUpdateDTO tagGroup)
        {
            throw new NotImplementedException();
        }
    }
}
