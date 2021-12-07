namespace ProjectBank.Core.DTOs
{
    public record ProjectDTO
    {
        public int Id { get; init; }

        [Required]
        public string Name { get; init; }

        [Required]
        [StringLength(400)]
        public string Description { get; init; }

        [Required]
        public ISet<TagDTO> Tags { get; init; }
        
        [Required]
        public ISet<UserDTO> Supervisors { get; set; }
    }

    public record ProjectCreateDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(400)]
        public string Description { get; set; }

        [Required]
        public ISet<int> ExistingTagIds { get; set; } = new HashSet<int>();

        [Required] public ISet<TagCreateDTO> NewTagDTOs { get; set; } = new HashSet<TagCreateDTO>();

        [Required]
        public ISet<int> UserIds { get; set; } = new HashSet<int>();
    }

    public record ProjectUpdateDTO : ProjectCreateDTO { }
}
