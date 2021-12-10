namespace ProjectBank.Infrastructure.Entities
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(400)]
        public string Description { get; set; }

        public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        public ICollection<User> Supervisors { get; set; } = new HashSet<User>();
    }
}
