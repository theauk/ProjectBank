namespace ProjectBank.Core.DTOs
{
    public record TagGroupDTO
    {
        public TagGroupDTO(int id, string? name, HashSet<int> hashSet, bool supervisorCanAddTag, bool requiredInProject, int tagLimit)
        {
            Id = id;
            Name = name;
            HashSet = hashSet;
            SupervisorCanAddTag = supervisorCanAddTag;
            RequiredInProject = requiredInProject;
            TagLimit = tagLimit;
        }

        public int Id { get; set; }

        [Required] 
        public string? Name { get; set; }

        public ISet<int>? tagIds { get; set; }

        [Required] 
        public bool SupervisorCanAddTag { get; set; }

        [Required] 
        public bool RequiredInProject { get; set; }

        public int TagLimit { get; set; }
        public HashSet<int> HashSet { get; } = new HashSet<int>();
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