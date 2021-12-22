namespace ProjectBank.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IProjectBankContext _context;

    public ProjectRepository(IProjectBankContext context) => _context = context;

    public async Task<Response> CreateAsync(ProjectCreateDTO project, string ownerEmail)
    {
        var university = await GetUniversityAsync(ownerEmail);
        var owner = await _context.Users.FirstOrDefaultAsync(u => u.Email == ownerEmail);
        var supervisors = await GetUsersAsync(project.UserIds);
        
        if (university == null || owner == null || supervisors == null)
            return Response.BadRequest;

        // Add owner as supervisors
        supervisors.Add(owner);

        // Add existing and new Tags to Project
        var tags = await GetTagsAsync(project.ExistingTagIds).ToSetAsync();
        tags.UnionWith(await CreateTagsAsync(project.NewTagDTOs).ToSetAsync());

        if (tags.Contains(null))
            return Response.BadRequest;

        var entity = new Project
        {
            Name = project.Name,
            Description = project.Description,
            Tags = tags,
            Supervisors = supervisors,
            University = university
        };

        _context.Projects.Add(entity);
        await _context.SaveChangesAsync();

        return Response.Created;
    }

    public async Task<Response> DeleteAsync(int projectId)
    {
        var entity = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

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

    public Task<IReadOnlyCollection<ProjectDTO>> ReadFilteredAsync(string email, IList<int> tagIds, IList<int> supervisorIds)
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

    public async Task<IReadOnlyCollection<ProjectDTO>> ReadAllByUniversityAsync(string email)
    {
        var domain = email.Split("@")[1];
        return (await _context.Projects
            .Select(p => p)
            .Include(p => p.Tags)
            .Include(p => p.Supervisors)
            .Where(p => p.University.DomainName == domain)
            .ToListAsync()).ToDTO().ToList().AsReadOnly();
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

        // Update Tags
        var tags = await GetTagsAsync(project.ExistingTagIds).ToSetAsync();
        tags.UnionWith(await CreateTagsAsync(project.NewTagDTOs).ToSetAsync());
        
        if (tags.Contains(null))
            return Response.BadRequest;

        entity.Tags = tags;

        // Update Supervisors
        var supervisors = await GetUsersAsync(project.UserIds);

        if (supervisors == null)
            return Response.BadRequest;

        entity.Supervisors = supervisors;

        await _context.SaveChangesAsync();

        return Response.Updated;
    }

    private async IAsyncEnumerable<Tag?> GetTagsAsync(IEnumerable<int> existingTagIds)
    {
        var existing = await _context.Tags.Where(t => existingTagIds.Contains(t.Id)).ToDictionaryAsync(t => t.Id);

        foreach (var tagId in existingTagIds)
            yield return existing.TryGetValue(tagId, out var t) ? t : null;
    }

    private async IAsyncEnumerable<Tag?> CreateTagsAsync(IEnumerable<TagCreateDTO> tags)
    {
        foreach (var tag in tags)
        {
            var tagGroup = await GetTagGroupAsync(tag.TagGroupId);

            if (tagGroup != null)
            {
                var t = new Tag { Value = tag.Value };
                tagGroup.Tags.Add(t);
                await _context.SaveChangesAsync();
                yield return t;
            }
            else 
                yield return null;
        }        
    }

    private async Task<TagGroup?> GetTagGroupAsync(int id) => await _context.TagGroups.FindAsync(id);

    private async Task<ISet<User>?> GetUsersAsync(IEnumerable<int> userIds)
    {
        var existing = await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id);
        var result = new HashSet<User>();

        foreach (var userId in userIds)
        {
            existing.TryGetValue(userId, out var user);
            if (user != null)
                result.Add(user);
            else
                return null;
        }

        return result;
    }

    private async Task<University?> GetUniversityAsync(string email) => string.IsNullOrWhiteSpace(email) ? null : await _context.Universities.FindAsync(email.Split("@")[1]);
}
