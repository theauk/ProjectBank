namespace ProjectBank.Infrastructure.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public University University { get; set; }

    public ICollection<Project> Projects { get; set; } = new HashSet<Project>(); // todo: this is never used???

    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
