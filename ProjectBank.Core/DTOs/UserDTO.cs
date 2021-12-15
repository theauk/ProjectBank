namespace ProjectBank.Core.DTOs;

public record UserDTO
{
    public int Id { get; init; }

    [Required]
    public string? Name { get; init; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public Role Role { get; set; }
}

public record UserCreateDTO
{
    [Required]
    public string? Name { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public Role Role { get; set; }
}
