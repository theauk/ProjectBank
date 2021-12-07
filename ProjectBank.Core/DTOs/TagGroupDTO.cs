namespace ProjectBank.Core.DTOs
{
    public record TagGroupDTO
    {
        public int Id { get; init; }

        [Required] public string Name { get; init; }

        public ISet<TagDTO> TagDTOs { get; init; }

        [Required] public bool SupervisorCanAddTag { get; init; }

        [Required] public bool RequiredInProject { get; init; }

        public int? TagLimit { get; init; }
    }

    public record TagGroupCreateDTO
    {
        [Required] public string Name { get; set; }

        public ISet<TagCreateDTO> TagCreateDTOs { get; set; }

        [Required] public bool SupervisorCanAddTag { get; set; } = true;

        [Required] public bool RequiredInProject { get; set; } = false;

        public int? TagLimit { get; set; }
    }

    public record TagGroupUpdateDTO : TagCreateDTO
    {
        
        [Required] public string Name { get; init; }

        public ISet<TagDTO> TagDTOs { get; init; }

        [Required] public bool SupervisorCanAddTag { get; init; }

        [Required] public bool RequiredInProject { get; init; }

        public int? TagLimit { get; init; }

        public ISet<int>? DeletedTagIds { get; set; } = new HashSet<int>();

        public ISet<TagCreateDTO>? NewTags { get; set; } = new HashSet<TagCreateDTO>();
    }
}