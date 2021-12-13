using System.Net.Mail;

namespace ProjectBank.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IProjectBankContext _context;

    public ProjectRepository(IProjectBankContext context) => _context = context;

        public async Task<Response> CreateAsync(ProjectCreateDTO project, string email, string name)
    {
        var (mainAndCoSupervisorsResponse, mainAndCoSupervisors) =
            await GetAllSupervisors(project, email, name);
        if (mainAndCoSupervisorsResponse == Response.BadRequest)
            return Response.BadRequest;

        var (responseTags, tags) =
            await AddTagsFromExistingTagGroupsReturnIds(project.NewTagDTOs);
        if (responseTags == Response.BadRequest)
            return Response.BadRequest;

        var entity = new Project
        {
            Name = project.Name,
            Description = project.Description,
            Tags = await GetTagsAsync(project.ExistingTagIds).ToSetAsync(),
            Supervisors = mainAndCoSupervisors
        };

        var updatedTags = entity.Tags.Concat(tags);
        entity.Tags = updatedTags.ToHashSet();

        _context.Projects.Add(entity);
        await _context.SaveChangesAsync();

        return Response.Created;
    }

    private async Task<(Response, ISet<User>)> GetAllSupervisors(
        ProjectCreateDTO project, string email, string name)
    {
        var (mainSupervisorResponse, mainSupervisor) =
            await GetMainSupervisor(email, name);
        if (mainSupervisorResponse == Response.BadRequest)
            return (Response.BadRequest, new HashSet<User>());

        var (responseCoSupervisors, coSupervisors) =
            await GetUsersAsync(project.UserIds);
        if (responseCoSupervisors == Response.BadRequest)
            return (Response.BadRequest, new HashSet<User>());

        var mainAndCoSupervisors = coSupervisors.ToHashSet();
        mainAndCoSupervisors.Add(mainSupervisor);
        return (Response.Success, mainAndCoSupervisors);
    }

    private async Task<(Response, User)> GetMainSupervisor(string email, string name)
    {
        var mainSupervisor = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.Equals(email));
        
        if (mainSupervisor == null)
        {
            if (!MailAddress.TryCreate(email, out _))
                return (Response.BadRequest, new User());

            var universityDomain = email.Split("@");
            var university = _context.Universities.
                FirstOrDefault(u => u.DomainName.Equals(universityDomain[1]));
            if (university == null)
                return (Response.BadRequest, new User());

            // Creation of users would normally already had happen at the initial setup based on the users and their roles in Azure.
            mainSupervisor = new User {Email = email, Name = name, University = university};
            _context.Users.Add(mainSupervisor);
        }

        return (Response.Success, mainSupervisor);
    }

    private async Task<(Response, IEnumerable<Tag>)> AddTagsFromExistingTagGroupsReturnIds(
        ISet<TagCreateDTO> projectNewTagDtos)
    {
        var newTags = new List<Tag>();

        foreach (var tag in projectNewTagDtos)
        {
            var tagGroup = await _context.TagGroups
                .FirstOrDefaultAsync(tg => tg.Id == tag.TagGroupId);
            if (tagGroup == null)
                return (Response.BadRequest, new List<Tag>());

            var newTag = new Tag {Value = tag.Value};
            tagGroup.Tags.Add(newTag);

            newTags.Add(newTag);
        }

        await _context.SaveChangesAsync();

        return (Response.Created, newTags);
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

        var (supervisorResponse, supervisors) = await GetUsersAsync(project.UserIds);

        if (supervisorResponse == Response.BadRequest)
            return Response.BadRequest;
        entity.Supervisors = supervisors.ToHashSet();

        await _context.SaveChangesAsync();

        return Response.Updated;
    }

    private async IAsyncEnumerable<Tag> GetTagsAsync(IEnumerable<int> tagIds)
    {
        var existing = await _context.Tags.Where(t => tagIds.Contains(t.Id)).ToDictionaryAsync(t => t.Id);

        foreach (var tagId in tagIds)
            yield return existing.TryGetValue(tagId, out var t) ? t : new Tag {Value = t.Value};
    }

    private async Task<(Response, IEnumerable<User>)> GetUsersAsync(IEnumerable<int> userIds)
    {
        var existing = await _context.Users.Where(u => userIds.Contains(u.Id)).ToDictionaryAsync(u => u.Id);
        var users = new List<User>();

        foreach (var userId in userIds)
        {
            var user = existing.TryGetValue(userId, out var u) ? u : null;
            if (user != null)
                users.Add(user);
            else
                return (Response.BadRequest, new List<User>());
        }

        return (Response.Success, users);
    }
}