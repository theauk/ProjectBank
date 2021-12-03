using ProjectBank.Infrastructure;
using ProjectBank.Core.DTOs;
using ProjectBank.Core.IRepositories;

namespace ProjectBank.Infrastructure.Repositories
{
    public class TagGroupRepository : ITagGroupRepository
    {
        private readonly IProjectBankContext _context;

        public TagGroupRepository(IProjectBankContext context)
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
