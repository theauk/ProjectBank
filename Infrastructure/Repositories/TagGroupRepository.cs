namespace Infrastructure
{
    public class TagGroupRepository : ITagGroupRepository
    {
        private readonly ProjectBankContext _context;

        public TagGroupRepository(ProjectBankContext context)
        {
            _context = context;
        }

        public Task<(Response, TagGroupDTO)> CreateAsync(TagGroupCreateDTO tagGroup)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(int tagGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<(Response, IReadOnlyCollection<TagGroupDTO>)> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<(Response, TagGroupDTO)> ReadAsync(int tagGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(int tagGroupId, TagGroupUpdateDTO tagGroup)
        {
            throw new NotImplementedException();
        }
    }
}
