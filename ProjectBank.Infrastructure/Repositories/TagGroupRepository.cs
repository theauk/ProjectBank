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
            var entity = new TagGroup
            {
                Name = tagGroup.Name,
                SupervisorCanAddTag = tagGroup.SupervisorCanAddTag,
                RequiredInProject = tagGroup.RequiredInProject,
                TagLimit = tagGroup.TagLimit,
                Tags = tagGroup.Tags.Select(t => new Tag(t.value)).ToHashSet(),
            };

            await _context.TagGroups.AddAsync(entity);
            await _context.SaveChangesAsync();

            return Response.Created;
        }

        public async Task<Response> DeleteAsync(int tagGroupId)
        {
            var entity = await _context.TagGroups.FindAsync(tagGroupId);

            if (entity == null) return Response.NotFound;

            _context.TagGroups.Remove(entity);
            await _context.SaveChangesAsync();

            return Response.Deleted;
        }

        public async Task<TagGroupDTO?> ReadAsync(int tagGroupId)
        {
            var tagGroup = await _context.TagGroups.FirstOrDefaultAsync(tg => tg.Id == tagGroupId);
            return tagGroup == null ? null : new TagGroupDTO(tagGroup.Id, tagGroup.Name, tagGroup.Tags.Select(t => t.Id).ToHashSet(), tagGroup.RequiredInProject, tagGroup.SupervisorCanAddTag, tagGroup.TagLimit);
        }

        public async Task<IReadOnlyCollection<TagGroupDTO>> ReadAllAsync()
        {
            return (await _context.TagGroups.Select(tg => new TagGroupDTO(tg.Id, tg.Name, tg.Tags.Select(t => t.Id).ToHashSet(), tg.SupervisorCanAddTag, tg.RequiredInProject, tg.TagLimit))
                .ToListAsync())
                .AsReadOnly();
        }

        public async Task<Response> UpdateAsync(int tagGroupId, TagGroupUpdateDTO tagGroup)
        {
            var entity = await _context.TagGroups.Include(tg => tg.Tags).FirstOrDefaultAsync(tg => tg.Id == tagGroupId);

            if (entity == null) return Response.NotFound;

            entity.Name = tagGroup.Name;
            entity.RequiredInProject = tagGroup.RequiredInProject;
            entity.SupervisorCanAddTag = tagGroup.SupervisorCanAddTag;
            entity.TagLimit = tagGroup.TagLimit;
            //entity.Tags = What to do?
            
            await _context.SaveChangesAsync();

            return Response.Updated;
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
