namespace ProjectBank.Infrastructure.Entities
{
    public class University
    {
        [Required]
        public string DomainName { get; set; }

        public ICollection<User> Users { get; set; } = new HashSet<User>();

        public ICollection<Project> Projects { get; set; } = new HashSet<Project>();

        public ICollection<TagGroup> TagGroups { get; set; } = new HashSet<TagGroup>();
    }
}
