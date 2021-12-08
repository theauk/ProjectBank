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
            var entity = new TagGroup
            {
                Name = tagGroup.Name,
                Tags = tagGroup.TagDTOs.Select(t => new Tag { Value = t.Value }).ToHashSet(),
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
            var entity = await _context.TagGroups.FirstOrDefaultAsync(tg => tg.Id == tagGroupId);

            if (entity == null)
            {
                return Response.NotFound;
            }

            _context.TagGroups.Remove(entity);
            await _context.SaveChangesAsync();

            return Response.Deleted;
        }

        public async Task<(Response, TagGroupDTO?)> ReadAsync(int tagGroupId)
        {
            var entity = await _context.TagGroups.FirstOrDefaultAsync(tg => tg.Id == tagGroupId);

            if (entity == null)
            {
                return (Response.NotFound, null);
            }

            return (Response.Success, new TagGroupDTO
            {
                Name = entity.Name,
                TagDTOs = entity.Tags.Select(t => new TagDTO { Id = t.Id, Value = t.Value }).ToHashSet(),
                SupervisorCanAddTag = entity.SupervisorCanAddTag,
                RequiredInProject = entity.RequiredInProject,
                TagLimit = entity.TagLimit
            });
        }

        public async Task<(Response, IReadOnlyCollection<TagGroupDTO>)> ReadAllAsync()
        {
            var tagGroups = (await _context.TagGroups.Select(tg => new TagGroupDTO
            {
                Name = tg.Name,
                TagDTOs = tg.Tags.Select(t => new TagDTO { Id = t.Id, Value = t.Value }).ToHashSet(),
                SupervisorCanAddTag = tg.SupervisorCanAddTag,
                RequiredInProject = tg.RequiredInProject,
                TagLimit = tg.TagLimit
            }).ToListAsync()).AsReadOnly();

            return (Response.Success, tagGroups);
        }

        public async Task<Response> UpdateAsync(int tagGroupId, TagGroupUpdateDTO tagGroup)
        {
            throw new NotImplementedException();
        }
    }
}
