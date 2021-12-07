namespace ProjectBank.Infrastructure
{
    public class University
    {
        public int Id { get; set; }
        [Required]
        public string? DomainName { get; set; }

        public ISet<User> Users { get; set; } = new HashSet<User>();

        public ISet<Project> Projects { get; set; } = new HashSet<Project>();

        public ISet<TagGroup> TagGroups { get; set; } = new HashSet<TagGroup>();
    }
}
