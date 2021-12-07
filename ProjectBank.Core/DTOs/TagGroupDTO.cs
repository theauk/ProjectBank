namespace ProjectBank.Core.DTOs
{
    public record TagGroupDTO
    {
        public int Id { get; init; }

        [Required]
        public string? Name { get; init; }

        public ISet<int> DeletedTagIds { get; init; } = new HashSet<int>();

        public ISet<TagCreateDTO> NewTags { get; init; } = new HashSet<TagCreateDTO>();

        public ISet<TagDTO> TagDTOs { get; init; } = new HashSet<TagDTO>();

        [Required]
        public bool SupervisorCanAddTag { get; init; }

        [Required]
        public bool RequiredInProject { get; init; }

        public int TagLimit { get; init; }
    }

    public record TagGroupCreateDTO
    {
        [Required]
        public string? Name { get; init; }

        public ISet<TagDTO> TagDTOs { get; init; } = new HashSet<TagDTO>();

        [Required]
        public bool SupervisorCanAddTag { get; init; }

        [Required]
        public bool RequiredInProject { get; init; }

        public int TagLimit { get; init; }
    }

    public record TagGroupUpdateDTO : TagGroupCreateDTO {}
}