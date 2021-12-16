namespace ProjectBank.Infrastructure;

public static class EntityExtension
{
    public static UserDTO ToDTO(this User user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        Name = user.Name,
        Role = user.Role
    };

    public static IEnumerable<UserDTO> ToDTO(this IEnumerable<User> users) =>
        users.Select(u => u.ToDTO()).OrderBy(u => u.Name);

    public static TagDTO ToDTO(this Tag tag) => new()
    {
        Id = tag.Id,
        Value = tag.Value,
    };

    public static IEnumerable<TagDTO> ToDTO(this IEnumerable<Tag> tags) =>
        tags.Select(t => t.ToDTO());

    public static ProjectDTO ToDTO(this Project project) => new()
    {
        Id = project.Id,
        Name = project.Name,
        Description = project.Description,
        Tags = project.Tags.ToDTO().OrderBy(t => t.Value).ToList(),
        Supervisors = project.Supervisors.ToDTO().ToHashSet(),
    };

    public static IEnumerable<ProjectDTO> ToDTO(this IEnumerable<Project> projects) =>
        projects.Select(p => p.ToDTO());

    public static TagGroupDTO ToDTO(this TagGroup tg) => new()
    {
        Id = tg.Id,
        Name = tg.Name,
        RequiredInProject = tg.RequiredInProject,
        SupervisorCanAddTag = tg.SupervisorCanAddTag,
        TagDTOs = tg.Tags.ToDTO().ToList(),
        TagLimit = tg.TagLimit,
    };

    public static IEnumerable<TagGroupDTO> ToDTO(this IEnumerable<TagGroup> tgs) =>
        tgs.Select(tg => tg.ToDTO());

    public static UniversityDTO ToDTO(this University uni) => new()
    {
        DomainName = uni.DomainName,
        Projects = uni.Projects.ToDTO().ToHashSet(),
        Users = uni.Users.ToDTO().ToHashSet(),
        TagGroups = uni.TagGroups.ToDTO().ToHashSet()
    };
}
