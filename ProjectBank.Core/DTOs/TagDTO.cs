namespace ProjectBank.Core.DTOs;

public record TagDTO
{
    public int Id { get; init; }

    [Required]
    public string Value { get; init; }

    public int TagGroupId { get; init; }
}

public record TagCreateDTO
{
    [Required]
    public string Value { get; init; }

    public int TagGroupId { get; init; }
}
