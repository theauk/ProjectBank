using ProjectBank.Infrastructure;
using ProjectBank.Core.DTOs;
using ProjectBank.Core.IRepositories;

namespace ProjectBank.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly IProjectBankContext _context;

        public TagRepository(IProjectBankContext context)
        {
            _context = context;
        }

        public async Task<(Response, TagDTO)> CreateAsync(TagCreateDTO tag)
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
