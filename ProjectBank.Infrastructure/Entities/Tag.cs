namespace ProjectBank.Infrastructure.Entities
{
    public class Tag 

    {
        public int Id { get; set; }

        [Required] [StringLength(100)] public string Value { get; set; }

        public TagGroup TagGroup { get; set; }

        public ICollection<Project> Projects { get; set; } = new List<Project>();

    }
}
