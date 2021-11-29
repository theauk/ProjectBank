namespace Infrastructure
{
    public class TagGroup
    {        
        public int Id { get; set; }
        
        [Required]
        public string? Name { get; set; }
        
        public ISet<Tag>? Tags { get; set; }

        public bool SupervisorCanAddTag { get; set;}

        public bool RequiredInProject { get; set;}

        public int TagLimit { get; set;} 
    }
}
