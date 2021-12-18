namespace ProjectBank.Infrastructure.Entities;

public class Tag : IComparable<Tag>
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string? Value { get; set; }

    [Required]
    public TagGroup? TagGroup { get; set; }

    public ICollection<Project> Projects { get; set; } = new List<Project>();

    public int CompareTo(Tag? other)
    {
        return Value.CompareTo(other?.Value);
    }
}
