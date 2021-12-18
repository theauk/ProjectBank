namespace ProjectBank.Core.DTOs;

public record TagGroupDTO
{
    public int Id { get; init; }

    [Required]
    public string? Name { get; init; }

    public IList<TagDTO> TagDTOs { get; init; } = new List<TagDTO>();

    [Required]
    public bool SupervisorCanAddTag { get; init; }

    [Required]
    public bool RequiredInProject { get; init; }

    public int? TagLimit { get; init; }
    
    
    public virtual bool Equals(TagGroupDTO? tg) 
    {
        if (tg == null)
            return false;
        return (
            Id.Equals(tg.Id) &&
            Name.Equals(tg.Name) &&
            TagDTOs.All(tg.TagDTOs.Contains) && TagDTOs.Count.Equals(tg.TagDTOs.Count) && // Currently we do not care about sequence
            // TagDTOs.SequenceEqual(tg.TagDTOs) &&
            SupervisorCanAddTag.Equals(tg.SupervisorCanAddTag) &&
            RequiredInProject.Equals(tg.RequiredInProject) &&
            TagLimit.Equals(tg.TagLimit)
        );
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public record TagGroupCreateDTO
{
    [Required]
    public string? Name { get; set; }

    public ISet<TagCreateDTO> NewTagsDTOs { get; set; } = new HashSet<TagCreateDTO>();

    [Required]
    public bool SupervisorCanAddTag { get; set; } = true;

    [Required]
    public bool RequiredInProject { get; set; } = false;

    public int? TagLimit { get; set; }
}

public record TagGroupUpdateDTO : TagGroupCreateDTO
{
    public int Id { get; init; }
    public ISet<string> SelectedTagValues { get; set; } = new HashSet<string>();

}
