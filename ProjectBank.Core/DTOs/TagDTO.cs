namespace ProjectBank.Core.DTOs;

public record TagDTO
{
    public int Id { get; init; }

    [Required]
    public string? Value { get; init; }
    
    public virtual bool Equals(TagDTO? t) 
    {
        if (t == null)
            return false;
        return (
            Id.Equals(t.Id) &&
            Value.Equals(t.Value));
    }
}

public record TagCreateDTO
{
    [Required]
    public string? Value { get; init; }

    public int TagGroupId { get; init; }
}
