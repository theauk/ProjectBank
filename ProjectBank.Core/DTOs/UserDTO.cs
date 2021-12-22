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
    
    
    public virtual bool Equals(UserDTO? u)
    {
        return u != null && Email.Equals(u.Email);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
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
