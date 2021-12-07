namespace ProjectBank.Infrastructure 
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set;}

        [Required]
        [StringLength(400)]
        public string? Description {get; set;}

        [Required]
        public ISet<Tag> Tags {get; set;} = new HashSet<Tag>();

        [Required]
        public ISet<User> Supervisors = new HashSet<User>();
    }
}
