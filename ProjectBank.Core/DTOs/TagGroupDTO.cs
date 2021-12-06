namespace ProjectBank.Core.DTOs
{
    public record TagGroupDTO
    {
        public int Id { get; set; }

        [Required] public string? Name { get; set; }

        public ISet<TagDTO>? TagDTOs { get; set; }

        [Required] public bool SupervisorCanAddTag { get; set; }

        [Required] public bool RequiredInProject { get; set; }

        public int TagLimit { get; set; }
    }

    public record TagGroupCreateDTO
    {
        [Required] public string? Name { get; set; }

        public ISet<TagCreateDTO>? TagCreateDTOs { get; set; }

        [Required] public bool SupervisorCanAddTag { get; set; }

        [Required] public bool RequiredInProject { get; set; }

        public int TagLimit { get; set; }
    }

    public record TagGroupUpdateDTO : TagCreateDTO
    {
    }
}