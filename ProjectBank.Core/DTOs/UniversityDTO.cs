namespace ProjectBank.Core.DTOs
{
    public record UniversityDTO
    {
        public int Id { get; init; }

        [Required] public string? DomainName { get; init; }
        public ISet<int>? UserIds { get; init; }
        public ISet<int>? ProjectIds { get; init; }
        public ISet<int>? TagGroupIds { get; init; }
    }

    public record UniversityCreateDTO
    {
        [Required] public string? DomainName { get; init; }
        public ISet<int>? Users { get; init; }
        public ISet<int>? ProjectIds { get; init; }
        public ISet<int>? TagGroupIds { get; init; }
    }

    public record UniversityUpdateDTO : UniversityCreateDTO
    {
    }
}