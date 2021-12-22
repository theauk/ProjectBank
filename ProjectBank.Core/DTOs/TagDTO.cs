namespace ProjectBank.Core.DTOs;

public record TagDTO
{
    public int Id { get; init; }

    [Required]
    [StringLength(100)]
    public string? Value { get; init; }
    
    public virtual bool Equals(TagDTO? t) 
    {
        if (t == null)
            return false;
        return (
            Id.Equals(t.Id) &&
            Value.Equals(t.Value));
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public record TagCreateDTO
{
    [Required]
    [StringLength(100)]
    public string? Value { get; init; }

    [Required]
    public int TagGroupId { get; init; }
}
