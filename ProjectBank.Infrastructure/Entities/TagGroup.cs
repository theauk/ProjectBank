namespace ProjectBank.Infrastructure.Entities
{
    public class TagGroup
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Tag> Tags { get; set; } = new SortedSet<Tag>();

        [Required]
        public bool SupervisorCanAddTag { get; set; }

        [Required]
        public bool RequiredInProject { get; set; }

        public int? TagLimit { get; set; }
    }
}
