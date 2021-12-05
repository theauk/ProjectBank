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
                Tags = tagGroup.Tags.Select(t => new Tag(t.Value)).ToHashSet(),
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
            return tagGroup == null ? null : new TagGroupDTO(tagGroup.Id, tagGroup.Name, tagGroup.Tags.Select(t => new TagDTO(){Id = t.Id, Value = t.Value}).ToHashSet(), tagGroup.RequiredInProject, tagGroup.SupervisorCanAddTag, tagGroup.TagLimit);
        }

        public async Task<IReadOnlyCollection<TagGroupDTO>> ReadAllAsync()
        {
            return (await _context.TagGroups.Select(tg => new TagGroupDTO(tg.Id, tg.Name, tg.Tags.Select(t => new TagDTO(){Id = t.Id, Value = t.Value}).ToHashSet(), tg.SupervisorCanAddTag, tg.RequiredInProject, tg.TagLimit))
                .ToListAsync())
                .AsReadOnly();
        }

        public async Task<Response> UpdateAsync(int tagGroupId, TagGroupUpdateDTO tagGroup, ISet<int> tagsToDelete, ISet<TagCreateDTO> tagsToAdd)
        {
            var entity = await _context.TagGroups.Include(tg => tg.Tags).FirstOrDefaultAsync(tg => tg.Id == tagGroupId);

            if (entity == null) return Response.NotFound;

            entity.Name = tagGroup.Name;
            entity.RequiredInProject = tagGroup.RequiredInProject;
            entity.SupervisorCanAddTag = tagGroup.SupervisorCanAddTag;
            entity.TagLimit = tagGroup.TagLimit;

            await DeleteTagAsync(tagGroupId, tagsToDelete);
            await AddTagAsync(tagGroupId, tagsToAdd);
            
            

            return Response.Updated;
        }
        
        public async Task<Response> DeleteTagAsync(int tagGroupId, ISet<int> tagsToDelete)
        {
            foreach (var id in tagsToDelete)
            {
                var entity = await _context.Tags.FindAsync(id);

                if (entity == null) return Response.Conflict;

                _context.Tags.Remove(entity);
            }
            await _context.SaveChangesAsync();

            return Response.Deleted;

        }
        
        public async Task<Response> AddTagAsync(int tagGroupId, ISet<TagCreateDTO> tagsToAdd)
        {
            var tagGroup = await _context.TagGroups.FirstOrDefaultAsync(tg => tg.Id == tagGroupId);
            if (tagGroup == null) return Response.NotFound;
            
            foreach (var tagCreateDto in tagsToAdd)
            {
                tagGroup.Tags.Add(new Tag(tagCreateDto.Value));
            }
            await _context.SaveChangesAsync();
            return Response.Created;
        }
    }
}
