namespace ProjectBank.Infrastructure.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public Role Role { get; set; }

    [Required]
    public University? University { get; set; }

    public ICollection<Project> Projects { get; set; } = new HashSet<Project>();
}
