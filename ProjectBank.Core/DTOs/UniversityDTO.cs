namespace ProjectBank.Core.DTOs
{
    public record UniversityDTO
    {
        public int Id { get; init; }

        [Required] 
        public string? DomainName { get; init; }
        
        public ISet<int> UserIds { get; init; } = new HashSet<int>();
        public ISet<int> ProjectIds { get; init; } = new HashSet<int>();
        public ISet<int> TagGroupIds { get; init; } = new HashSet<int>();
    }

    public record UniversityCreateDTO
    {
        [Required] 
        public string? DomainName { get; init; }
    }

    public record UniversityUpdateDTO : UniversityCreateDTO
    {
        public ISet<int> Users { get; init; } = new HashSet<int>();
        public ISet<int> ProjectIds { get; init; } = new HashSet<int>();
        public ISet<int> TagGroupIds { get; init; } = new HashSet<int>();
    }
}