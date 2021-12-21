namespace ProjectBank.Infrastructure.Repositories;

public class TagGroupRepository : ITagGroupRepository
{
    private readonly IProjectBankContext _context;

    public TagGroupRepository(IProjectBankContext context) => _context = context;
    
    /// <summary>
    /// Adds a Tag Group to the database.
    /// </summary>
    /// <param name="tagGroup">Object to be added.</param>
    /// <param name="ownerEmail">Email of the user adding the Tag Group (for finding their university).</param>
    /// <returns><see cref="Response.Created"/> upon success, or <see cref="Response.BadRequest"/> if given invalid information.</returns>
    public async Task<Response> CreateAsync(TagGroupCreateDTO tagGroup, string ownerEmail)
    {
        var university = await GetUniversityAsync(ownerEmail);

        if (university == null)
            return Response.BadRequest;

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

    /// <summary>
    /// Removes a Tag Group from the database.
    /// </summary>
    /// <param name="tagGroupId"></param>
    /// <returns><see cref="Response.Deleted"/> upon success, or <see cref="Response.NotFound"/> if the given ID does not exist.</returns>
    public async Task<Response> DeleteAsync(int tagGroupId)
    {
        var entity = await _context.TagGroups.FindAsync(tagGroupId);

        if (entity == null)
            return Response.NotFound;

        _context.TagGroups.Remove(entity);
        await _context.SaveChangesAsync();

        return Response.Deleted;
    }

    /// <summary>
    /// Finds a specific Tag Group.
    /// </summary>
    /// <param name="tagGroupId">ID of the Tag Group you're looking for.</param>
    /// <returns>A <see cref="TagGroupDTO"/>, or null if ID is not found.</returns>
    public async Task<Option<TagGroupDTO>> ReadAsync(int tagGroupId) => (await _context.TagGroups
        .Include(tg => tg.Tags)
        .FirstOrDefaultAsync(tg => tg.Id == tagGroupId))?.ToDTO();

    /// <summary>
    /// Reads all Tag Groups in the database.
    /// Is only meant to be accessible to System Administrators.
    /// </summary>
    /// <returns>A List of all <see cref="TagGroupDTO"/>s in the database.</returns>
    public async Task<IReadOnlyCollection<TagGroupDTO>> ReadAllAsync() =>
        (await _context.TagGroups.Select(tg => tg).ToListAsync()).ToDTO().ToList().AsReadOnly();

    /// <summary>
    /// Gets all Tag Groups of a Universities.
    /// </summary>
    /// <param name="email">Email of user, who belongs to the university.</param>
    /// <returns>A list of all <see cref="TagGroupDTO"/>s belonging to a university.</returns>
    public async Task<IReadOnlyCollection<TagGroupDTO>> ReadAllByUniversityAsync(string email)
    {
        var domain = email.Split("@")[1];
        return (await _context.TagGroups.Select(tg => tg)
            .Include(tg => tg.Tags)
            .Where(tg => tg.University.DomainName == domain)
            .ToListAsync()).ToDTO().ToList().AsReadOnly();
    }

    /// <summary>
    /// Updates a Tag Group within the database.
    /// </summary>
    /// <param name="tagGroupId">ID of the Tag Group in question.</param>
    /// <param name="tagGroup">New values of the Tag Group</param>
    /// <returns><see cref="Response.Updated"/> if successful, <see cref="Response.NotFound"/> if no Tag Group with the ID exists or <see cref="Response.BadRequest"/> if unable to delete Tags.</returns>
    public async Task<Response> UpdateAsync(int tagGroupId, TagGroupUpdateDTO tagGroup)
    {
        var entity = await _context.TagGroups
            .Include(tg => tg.Tags)
            .FirstOrDefaultAsync(tg => tg.Id == tagGroupId);

        if (entity == null)
            return Response.NotFound;

        //get rid of deleted tags
        var oldTagGroupDto = (await ReadAsync(tagGroupId)).Value;
        var deletedTagIds = (from tagDTO in oldTagGroupDto?.TagDTOs
            where !tagGroup.SelectedTagValues.Contains(tagDTO.Value)
            select tagDTO.Id).ToList();

        // Delete the range of tags.
        if (await DeleteTagAsync(deletedTagIds) == Response.BadRequest)
            return Response.BadRequest;

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
    
    /// <summary>
    /// Removes a Range of tags from the database.
    /// </summary>
    /// <param name="tagsToDelete">List of tag IDs for removal.</param>
    /// <returns><see cref="Response.Deleted"/> if successful, <see cref="Response.BadRequest"/> if unable to find a tag in the list.</returns>
    private async Task<Response> DeleteTagAsync(IEnumerable<int> tagsToDelete)
    {
        foreach (var id in tagsToDelete)
        {
            var entity = await _context.Tags.FindAsync(id);

            if (entity == null) 
                return Response.BadRequest;

            _context.Tags.Remove(entity);
        }

        await _context.SaveChangesAsync();

        return Response.Deleted;
    }

    /// <summary>
    /// Adds a range of Tag entities to the database.
    /// </summary>
    /// <param name="tagGroupId">ID to which the tags belong.</param>
    /// <param name="tagsToAdd">List of Tags to create.</param>
    /// <returns><see cref="Response.Created"/> if successful.</returns>
    private async Task<Response> AddTagAsync(int tagGroupId, IEnumerable<TagCreateDTO> tagsToAdd)
    {
        var tagGroup = await _context.TagGroups.FindAsync(tagGroupId);

        foreach (var tagCreateDto in tagsToAdd)
            tagGroup?.Tags.Add(new Tag {Value = tagCreateDto.Value});

        return Response.Created;
    }
    
    /// <summary>
    /// Returns the university entity with the same domain as the as the <paramref name="email"/>.
    /// </summary>
    /// <param name="email">Email address of the user who's university you are trying to find.</param>
    /// <returns>A university object, with the same domain as <paramref name="email"/>, or null, if no such university could be found.</returns>
    private async Task<University?> GetUniversityAsync(string email) =>
        string.IsNullOrWhiteSpace(email) ? null : await _context.Universities.FindAsync(email.Split("@")[1]);
}