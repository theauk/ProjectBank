namespace ProjectBank.Core.DTOs;

public record TagDTO
{
    public int Id { get; init; }

    [Required]
    public string? Value { get; init; }

    public int TagGroupId { get; init;} //TODO Tag stilling (også nede i Equals): Always 0 - Currently we do not implement the usage/init of TagGroupId in record TagGroup anywhere - but can be useful if we want to sort tags by taggroup on projectpage
    
    public virtual bool Equals(TagDTO? t) 
    {
        if (t == null)
            return false;
        return (
            Id.Equals(t.Id) &&
            Value.Equals(t.Value) 
            // && TagGroupId.Equals(t.TagGroupId)  // Currently we do not implement the usage of TagGroupId in record TagGroup anywhere - but can be useful for sorting tags by taggroup
        );
    }
}

public record TagCreateDTO
{
    [Required]
    public string? Value { get; init; }

    public int TagGroupId { get; init; }
}
