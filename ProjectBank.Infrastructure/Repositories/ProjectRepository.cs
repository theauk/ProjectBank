using System.Net.Mail;

namespace ProjectBank.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IProjectBankContext _context;

    public ProjectRepository(IProjectBankContext context)
    {
        _context = context;
    }

    public async Task<Response> CreateAsync(ProjectCreateDTO project, string email, string name)
    {
        var (mainAndCoSupervisorsResponse, mainAndCoSupervisors) = await GetAllSupervisors(project, email, name);
        if (mainAndCoSupervisorsResponse == Response.BadRequest) return Response.BadRequest;

        var (responseTags, tags) = await AddTagsFromExistingTagGroupsReturnIds(project.NewTagDTOs);
        if (responseTags == Response.BadRequest) return Response.BadRequest;

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

    private async Task<(Response, ISet<User>)> GetAllSupervisors(ProjectCreateDTO project, string email,
        string name)
    {
        var (mainSupervisorResponse, mainSupervisor) = await GetMainSupervisor(email, name);
        if (mainSupervisorResponse == Response.BadRequest) return (Response.BadRequest, new HashSet<User>());

        var (responseCoSupervisors, coSupervisors) = await GetUsersAsync(project.UserIds);
        if (responseCoSupervisors == Response.BadRequest) return (Response.BadRequest, new HashSet<User>());

        var mainAndCoSupervisors = coSupervisors.ToHashSet();
        mainAndCoSupervisors.Add(mainSupervisor);
        return (Response.Success, mainAndCoSupervisors);
    }

    private async Task<(Response, User)> GetMainSupervisor(string email, string name)
    {
        var mainSupervisor = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));

        if (mainSupervisor == null)
        {
            if (!MailAddress.TryCreate(email, out var _))
                return (Response.BadRequest, new User());

            var universityDomain = email.Split("@");
            var university = _context.Universities.FirstOrDefault(u => u.DomainName.Equals(universityDomain[1]));
            if (university == null) return (Response.BadRequest, new User());

            // Creation of users would normally already had happen at the initial setup based on the users and their roles in Azure.
            mainSupervisor = new User() {Email = email, Name = name, University = university};
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
            var tagGroup = await _context.TagGroups.FirstOrDefaultAsync(tg => tg.Id == tag.TagGroupId);
            if (tagGroup == null) return (Response.BadRequest, new List<Tag>());

            var newTag = new Tag {Value = tag.Value};
            tagGroup.Tags.Add(newTag);

            newTags.Add(newTag);
        }

        await _context.SaveChangesAsync();

        return (Response.Created, newTags);
    }

    public async Task<Response> DeleteAsync(int projectId)
    {
        var entity = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

        if (entity == null)
        {
            return Response.NotFound;
        }

        _context.Projects.Remove(entity);
        await _context.SaveChangesAsync();

        return Response.Deleted;
    }

    public async Task<Option<ProjectDTO?>> ReadAsync(int projectId)
    {
        if (_context.Projects.FirstOrDefault(p => p.Id == projectId) == null)
            return null; //ToDo Find en anden løsning end den her quickfix - Jeg tror at det har noget med Select på null at gøre

        var project = (await _context.Projects.Where(p => (p.Id == projectId)).Select(p =>
            new ProjectDTO() // ToDo Kan ikke få den her implementation til at håndtere 404 response, heraf løsning med FirstOrDefault
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Tags = p.Tags.Select(t => new TagDTO {Id = t.Id, Value = t.Value}).OrderBy(t => t.Value)
                        .ToList(),
                    Supervisors = p.Supervisors.Select(u => new UserDTO {Id = u.Id, Name = u.Name}).ToHashSet()
                }).ToListAsync()).First();

        return project;

        // var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId); // ToDO FirstOrDefaultAsync kan ikke håndtere, at vi referere til associated tags og superviser i henholdvis ProjectTag og ProjectUser
        // {
        //     Id = project.Id,
        //     Name = project.Name,
        //     Description = project.Description,
        //     Tags = project.Tags.Select(t => new TagDTO { Id = t.Id, Value = t.Value }).OrderBy(t => t.Value).ToList(), //ToDo Kan ikke få en liste af tags eller supervisors ud i ProjectPage Console -
        //     Supervisors = project.Supervisors.Select(u => new UserDTO { Id = u.Id, Name = u.Name }).ToHashSet()
        // };
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync()
    {
        var projects = (await _context.Projects.Select(p => new ProjectDTO
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Tags = p.Tags.Select(t => new TagDTO {Id = t.Id, Value = t.Value}).OrderBy(t => t.Value).ToList(),
            Supervisors = p.Supervisors.Select(user => new UserDTO {Id = user.Id, Name = user.Name}).ToHashSet()
        }).ToListAsync()).AsReadOnly();

        return projects;
    }


    public Task<IReadOnlyCollection<ProjectDTO>> ReadFilteredAsync(IList<int> tagIds, IList<int> supervisorIds)
    {
        var projects = _context.Projects.Select(p => new ProjectDTO
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Tags = p.Tags.Select(t => new TagDTO {Id = t.Id, Value = t.Value}).OrderBy(t => t.Value).ToList(),
            Supervisors = p.Supervisors.Select(user => new UserDTO {Id = user.Id, Name = user.Name}).ToHashSet(),
        }).ToList();

        if (tagIds.Any())
            projects = projects.Where(p => tagIds.All(tId => p.Tags.Select(t => t.Id).Contains(tId))).ToList();
        if (supervisorIds.Any())
            projects = projects.Where(p => supervisorIds.All(sId => p.Supervisors.Select(s => s.Id).Contains(sId)))
                .ToList();

        return Task.FromResult<IReadOnlyCollection<ProjectDTO>>(projects.AsReadOnly());
    }

    public async Task<Response> UpdateAsync(int projectId, ProjectUpdateDTO project)
    {
        var entity = await _context.Projects.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == projectId);

        if (entity == null)
        {
            return Response.NotFound;
        }

        entity.Name = project.Name;
        entity.Description = project.Description;
        entity.Tags = await GetTagsAsync(project.ExistingTagIds).ToSetAsync();

        var (supervisorResponse, supervisors) = await GetUsersAsync(project.UserIds);

        if (supervisorResponse == Response.BadRequest) return Response.BadRequest;
        entity.Supervisors = supervisors.ToHashSet();

        await _context.SaveChangesAsync();

        return Response.Updated;
    }

    private async IAsyncEnumerable<Tag> GetTagsAsync(IEnumerable<int> tagIds)
    {
        var existing = await _context.Tags.Where(t => tagIds.Contains(t.Id)).ToDictionaryAsync(t => t.Id);

        foreach (var tagId in tagIds)
        {
            yield return existing.TryGetValue(tagId, out var t) ? t : new Tag {Value = t.Value};
        }
    }

    private async Task<(Response, IEnumerable<User>)> GetUsersAsync(IEnumerable<int> userIds)
    {
        var existing = await _context.Users.Where(u => userIds.Contains(u.Id)).ToDictionaryAsync(u => u.Id);
        var users = new List<User>();

        foreach (var userId in userIds)
        {
            var user = existing.TryGetValue(userId, out var u) ? u : null;
            if (user != null) users.Add(user);
            else return (Response.BadRequest, new List<User>());
        }

        return (Response.Success, users);
    }
}