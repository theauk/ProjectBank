namespace ProjectBank.Core.DTOs
{
    public record TagGroupDTO
    {
        public int Id { get; init; }

        [Required] public string Name { get; init; }

        public List<TagDTO> TagDTOs { get; init; } = new List<TagDTO>();

        [Required]
        public bool SupervisorCanAddTag { get; init; }

        [Required]
        public bool RequiredInProject { get; init; }

        public int? TagLimit { get; init; }
    }

    public record TagGroupCreateDTO
    {
        [Required]
        public string Name { get; set; }

        public ISet<TagCreateDTO> NewTagsDTOs { get; set; } = new HashSet<TagCreateDTO>();

        [Required] public bool SupervisorCanAddTag { get; set; } = true;

        [Required] public bool RequiredInProject { get; set; } = false;

        public int? TagLimit { get; set; }
    }



    public record TagGroupUpdateDTO : TagGroupCreateDTO
    {
        public int Id { get; init; }
        
        public ISet<string> SelectedTagValues { get; set; } = new HashSet<string>();

    }
}