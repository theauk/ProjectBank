namespace Core
{
    public record TagGroupDTO
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public ISet<TagDTO>? TagDTOs { get; set; }

        [Required]
        public bool SupervisorCanAddTag { get; set; }

        [Required]
        public bool RequiredInProject { get; set; }

        public int TagLimit { get; set; }

    }

    public record TagGroupCreateDTO
    {
        [Required]
        public string? Name { get; init; }

        public ISet<TagDTO>? TagDTOs { get; init; }

        [Required]
        public bool SupervisorCanAddTag { get; init; }

        [Required]
        public bool RequiredInProject { get; init; }

        public int TagLimit { get; init; }
    }

    public record TagGroupUpdateDTO : TagCreateDTO { }
}
