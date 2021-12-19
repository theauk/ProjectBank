namespace ProjectBank.Infrastructure.Repositories;

public class TagGroupRepository : ITagGroupRepository
{
    private readonly IProjectBankContext _context;

    public TagGroupRepository(IProjectBankContext context) => _context = context;

    public async Task<Response> CreateAsync(TagGroupCreateDTO tagGroup, string ownerEmail)
    {
        var university = await GetUniversityAsync(ownerEmail);

        if (university == null)
        {
            return Response.BadRequest;
        }

        var entity = new TagGroup
        {
            Name = tagGroup.Name,
            SupervisorCanAddTag = tagGroup.SupervisorCanAddTag,
            RequiredInProject = tagGroup.RequiredInProject,
            TagLimit = tagGroup.TagLimit,
            Tags = tagGroup.NewTagsDTOs.Select(t => new Tag {Value = t.Value}).ToHashSet(),
            University = university
        };

        _context.TagGroups.Add(entity);
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

    public async Task<Option<TagGroupDTO>> ReadAsync(int tagGroupId)
    {
        var tagGroup = await _context.TagGroups.Include(tg => tg.Tags).FirstOrDefaultAsync(tg => tg.Id == tagGroupId);
        return tagGroup == null ? null : tagGroup.ToDTO();
    }

    public async Task<IReadOnlyCollection<TagGroupDTO>> ReadAllAsync()
    {
        return (await _context.TagGroups.Select(tg => tg).ToListAsync()).ToDTO().ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<TagGroupDTO>> ReadAllByUniversityAsync(string email)
    {
        var domain = email.Split("@")[1];
        return (await _context.TagGroups.Select(tg => tg).Include(tg => tg.Tags).Where(tg => tg.University.DomainName == domain).ToListAsync()).ToDTO().ToList().AsReadOnly();
    }

    public async Task<Response> UpdateAsync(int tagGroupId, TagGroupUpdateDTO tagGroup)
    {
        var entity = await _context.TagGroups.Include(tg => tg.Tags).FirstOrDefaultAsync(tg => tg.Id == tagGroupId);

        if (entity == null) return Response.NotFound;

        //get rid of deleted tags
        var oldTagGroupDto = (await ReadAsync(tagGroupId)).Value;
        var deletedTagIds = (from tagDTO in oldTagGroupDto?.TagDTOs
            where !tagGroup.SelectedTagValues.Contains(tagDTO.Value)
            select tagDTO.Id).ToList();

        var deleteResponse = await DeleteTagAsync(deletedTagIds);
        if (deleteResponse == Response.BadRequest)
        {
            return deleteResponse;
        }

        //Create new tags
        var newTagDTOs = (from tagValue in tagGroup.SelectedTagValues
            where !oldTagGroupDto.TagDTOs.Select(t => t.Value).ToList().Contains(tagValue)
            select new TagCreateDTO {TagGroupId = tagGroupId, Value = tagValue}).ToList();

        await AddTagAsync(tagGroupId, newTagDTOs);

        entity.Name = tagGroup.Name;
        entity.RequiredInProject = tagGroup.RequiredInProject;
        entity.SupervisorCanAddTag = tagGroup.SupervisorCanAddTag;
        entity.TagLimit = tagGroup.TagLimit;

        await _context.SaveChangesAsync();

        return Response.Updated;
    }

    private async Task<Response> DeleteTagAsync(IEnumerable<int> tagsToDelete)
    {
        foreach (var id in tagsToDelete)
        {
            var entity = await _context.Tags.FindAsync(id);

            if (entity == null) return Response.BadRequest;

            _context.Tags.Remove(entity);
        }

        await _context.SaveChangesAsync();

        return Response.Deleted;
    }

    private async Task<Response> AddTagAsync(int tagGroupId, IEnumerable<TagCreateDTO> tagsToAdd)
    {
        var tagGroup = await _context.TagGroups.FindAsync(tagGroupId);

        foreach (var tagCreateDto in tagsToAdd)
        {
            tagGroup?.Tags.Add(new Tag {Value = tagCreateDto.Value});
        }

        return Response.Created;
    }

    private async Task<University?> GetUniversityAsync(string email) 
    {
        return string.IsNullOrWhiteSpace(email) ? null : await _context.Universities.FindAsync(email.Split("@")[1]);
    }
}