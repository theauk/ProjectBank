namespace ProjectBank.Infrastructure.Entities
{
    public class TagGroup
    {        
        public int Id { get; set; }
        
        [Required]
        public string? Name { get; set; }

        public ISet<Tag> Tags { get; set; } = new HashSet<Tag>();

        [Required]
        public bool SupervisorCanAddTag { get; set;}

        [Required]
        public bool RequiredInProject { get; set;}

        public int? TagLimit { get; set;} 
    }
}
