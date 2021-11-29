namespace Core
{
    public record TagGroupDTO
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public ISet<TagDTO>? TagDTOs { get; set; }

        public bool SupervisorCanAddTag { get; set; }

        public bool RequiredInProject { get; set; }

        public int TagLimit { get; set; }

    }

    public record TagGroupCreateDTO
    {
        [Required]
        public string? Name { get; init; }

        public ISet<TagDTO>? TagDTOs { get; init; }

        public bool SupervisorCanAddTag { get; init; }

        public bool RequiredInProject { get; init; }

        public int TagLimit { get; init; }
    }

    public record TagGroupUpdateDTO : TagCreateDTO  
    {
    }
}