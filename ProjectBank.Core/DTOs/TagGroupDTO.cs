namespace ProjectBank.Core.DTOs
{
    public record TagGroupDTO
    {
        public TagGroupDTO(int id, string? name, HashSet<TagDTO> tags, bool supervisorCanAddTag, bool requiredInProject, int tagLimit)
        {
            Id = id;
            Name = name;
            Tags   = tags;
            SupervisorCanAddTag = supervisorCanAddTag;
            RequiredInProject = requiredInProject;
            TagLimit = tagLimit;
        }

        public int Id { get; set; }

        [Required] 
        public string? Name { get; set; }

        public ISet<int>? deletedTagIds { get; set; }

        public ISet<TagCreateDTO>? newTags { get; set; }

        [Required] 
        public bool SupervisorCanAddTag { get; init; }

        [Required] 
        public bool RequiredInProject { get; init; }

        public int TagLimit { get; init; }
        public HashSet<TagDTO> Tags { get; set; } = new HashSet<TagDTO>();
    }

    public record TagGroupCreateDTO
    {
        [Required] 
        public string? Name { get; init; }

        public ISet<TagCreateDTO> Tags { get; init; } = new HashSet<TagCreateDTO>();

        [Required] 
        public bool SupervisorCanAddTag { get; init; }

        [Required] 
        public bool RequiredInProject { get; init; }

        public int TagLimit { get; init; }
    }

    public record TagGroupUpdateDTO : TagGroupCreateDTO {}
}