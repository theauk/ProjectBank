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

        public async Task<Response> CreateAsync(TagGroupCreateDTO tagGroup)
        {
            var tags = new HashSet<Tag>();
            foreach (var tagDto in tagGroup.TagDTOs)
            {
                var tag = new Tag
                {
                    Value = tagDto.Value
                };
                tags.Add(tag);
            }

            var entity = new TagGroup
            {
                Name = tagGroup.Name,
                Tags = tags,
                SupervisorCanAddTag = tagGroup.SupervisorCanAddTag,
                RequiredInProject = tagGroup.RequiredInProject,
                TagLimit = tagGroup.TagLimit
            };
            _context.TagGroups.Add(entity);

            await _context.SaveChangesAsync();

            return Response.Created;
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
        
        public async Task<Response> DeleteTagAsync(int tagGroupId, int tagId)
        {
            throw new NotImplementedException();
        }
        
        public async Task<Response> AddTagAsync(int tagGroupId, int tagId)
        {
            throw new NotImplementedException();
        }
    }
}
