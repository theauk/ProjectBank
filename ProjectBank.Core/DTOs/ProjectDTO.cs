namespace ProjectBank.Core.DTOs;

public record ProjectDTO
{
    public int Id { get; init; }

    [Required]
    public string? Name { get; init; }

    [Required]
    [StringLength(400)]
    public string? Description { get; init; }

    [Required]
    public ICollection<TagDTO> Tags { get; init; } = new List<TagDTO>();

    [Required]
    public ISet<UserDTO> Supervisors { get; init; } = new HashSet<UserDTO>();
    
    public virtual bool Equals(ProjectDTO? p)
    {
        if (p == null)
            return false;
        
        if (Supervisors.Count != p.Supervisors.Count) //
            return false;
        if (Supervisors.Any(super => !p.Supervisors.ToList().Contains(super)))
        {
            return false;
        }
        return (
            Id.Equals(p.Id) &&
            Name.Equals(p.Name) &&
            Description.Equals(p.Description) &&
            Tags.SequenceEqual(p.Tags)
        );
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public record ProjectCreateDTO
{
    [Required]
    public string? Name { get; set; }

    [Required]
    [StringLength(400)]
    public string? Description { get; set; }

    [Required]
    public ISet<int> ExistingTagIds { get; set; } = new HashSet<int>();

    [Required]
    public ISet<TagCreateDTO> NewTagDTOs { get; set; } = new HashSet<TagCreateDTO>();

    [Required]
    public ISet<int> UserIds { get; set; } = new HashSet<int>();
}

public record ProjectUpdateDTO : ProjectCreateDTO
{
    public int Id { get; init; }
}
