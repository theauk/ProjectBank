namespace ProjectBank.Core.DTOs;

public record UniversityDTO
{
    [Required]
    public string DomainName { get; init; }

    public ISet<UserDTO> Users { get; init; } = new HashSet<UserDTO>();
    public ISet<ProjectDTO> Projects { get; init; } = new HashSet<ProjectDTO>();
    public ISet<TagGroupDTO> TagGroups { get; init; } = new HashSet<TagGroupDTO>();
}

public record UniversityCreateDTO
{
    [Required]
    public string DomainName { get; init; }
}

public record UniversityUpdateDTO : UniversityCreateDTO
{
    public ISet<UserCreateDTO> Users { get; init; } = new HashSet<UserCreateDTO>();
    public ISet<ProjectCreateDTO> ProjectIds { get; init; } = new HashSet<ProjectCreateDTO>();
    public ISet<TagGroupCreateDTO> TagGroupIds { get; init; } = new HashSet<TagGroupCreateDTO>();
}
