namespace ProjectBank.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IProjectBankContext _context;

    public ProjectRepository(IProjectBankContext context) => _context = context;

    public async Task<Response> CreateAsync(ProjectCreateDTO project, string ownerEmail)
    {
        var owner = await GetUserAsync(ownerEmail);
        var supervisors = await GetUsersAsync(project.UserIds);
        
        if (owner == null || supervisors == null)
        {
            return Response.BadRequest;
        }

        // Add owner to supervisors
        supervisors.Add(owner);

        var (responseTags, tags) = await AddTagsFromExistingTagGroupsReturnIds(project.NewTagDTOs);

        if (responseTags == Response.BadRequest)
        {
            return Response.BadRequest;
        }

        var entity = new Project
        {
            Name = project.Name,
            Description = project.Description,
            Tags = await GetTagsAsync(project.ExistingTagIds).ToSetAsync(),
            Supervisors = supervisors
        };

        var updatedTags = entity.Tags.Concat(tags);
        entity.Tags = entity.Tags.Concat(tags).ToHashSet();

        _context.Projects.Add(entity);
        await _context.SaveChangesAsync();

        return Response.Created;
    }

    public async Task<Response> DeleteAsync(int projectId)
    {
        var entity = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (entity == null)
            return Response.NotFound;

        _context.Projects.Remove(entity);
        await _context.SaveChangesAsync();

        return Response.Deleted;
    }

    public async Task<Option<ProjectDTO>> ReadAsync(int projectId) => (await _context.Projects
        .Include(p => p.Supervisors)
        .Include(p => p.Tags)
        .Where(p => p.Id == projectId)
        .FirstOrDefaultAsync())?.ToDTO();

    public async Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync() => (await _context.Projects
        .Include(p => p.Tags)
        .Include(p => p.Supervisors)
        .ToListAsync()).ToDTO().ToList().AsReadOnly();

    public Task<IReadOnlyCollection<ProjectDTO>> ReadFilteredAsync(IList<int> tagIds, IList<int> supervisorIds)
    {
        var projects = _context.Projects
            .Include(p => p.Supervisors)
            .Include(p => p.Tags)
            .ToDTO();

        if (tagIds.Any())
            projects = projects.Where(p =>
                tagIds.All(tId => p.Tags.Select(t => t.Id).Contains(tId)));
        if (supervisorIds.Any())
            projects = projects.Where(p =>
                supervisorIds.All(sId => p.Supervisors.Select(s => s.Id).Contains(sId)));

        return Task.FromResult<IReadOnlyCollection<ProjectDTO>>(projects.ToList().AsReadOnly());
    }

    public async Task<Response> UpdateAsync(int projectId, ProjectUpdateDTO project)
    {
        var entity = await _context.Projects
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (entity == null)
            return Response.NotFound;

        entity.Name = project.Name;
        entity.Description = project.Description;
        entity.Tags = await GetTagsAsync(project.ExistingTagIds).ToSetAsync();

        var supervisors = await GetUsersAsync(project.UserIds);

        if (supervisors == null)
            return Response.BadRequest;

        entity.Supervisors = supervisors;

        await _context.SaveChangesAsync();

        return Response.Updated;
    }

    private async Task<(Response, IEnumerable<Tag>)> AddTagsFromExistingTagGroupsReturnIds(ISet<TagCreateDTO> projectNewTagDtos)
    {
        var newTags = new List<Tag>();

        foreach (var tag in projectNewTagDtos)
        {
            var tagGroup = await _context.TagGroups
                .FirstOrDefaultAsync(tg => tg.Id == tag.TagGroupId);
            if (tagGroup == null)
                return (Response.BadRequest, new List<Tag>());

            var newTag = new Tag { Value = tag.Value };
            tagGroup.Tags.Add(newTag);

            newTags.Add(newTag);
        }

        await _context.SaveChangesAsync();

        return (Response.Created, newTags);
    }

    private async IAsyncEnumerable<Tag> GetTagsAsync(IEnumerable<int> tagIds)
    {
        var existing = await _context.Tags.Where(t => tagIds.Contains(t.Id)).ToDictionaryAsync(t => t.Id);

        foreach (var tagId in tagIds)
            yield return existing.TryGetValue(tagId, out var t) ? t : new Tag { Value = t?.Value };
    }

    private async Task<ISet<User>?> GetUsersAsync(IEnumerable<int> userIds)
    {
        var existing = await _context.Users.Where(u => userIds.Contains(u.Id)).ToDictionaryAsync(u => u.Id);
        var result = new HashSet<User>();

        foreach (var userId in userIds)
        {
            existing.TryGetValue(userId, out var user);
            if (user != null)
            {
                result.Add(user);
            }
            else
            {
                return null;
            }
        }

        return result;
    }

    private async Task<User?> GetUserAsync(string? email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user == null ? null : user;
    }
}
