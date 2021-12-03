namespace ProjectBank.Infrastructure
{
    public class University 
    {
        public int Id {get; set;}
        [Required]
        public string? DomainName {get;set;}

        public ISet<User>? Users {get;set;}

        public ISet<Project>? Projects {get; set;}

        public ISet<TagGroup>? TagGroup {get; set;}
    }
}
