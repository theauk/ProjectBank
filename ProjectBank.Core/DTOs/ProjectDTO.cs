namespace ProjectBank.Core.DTOs
{
    public record ProjectDTO
    {
        public int Id { get; init; }

        [Required]
        public string Name { get; init; }

        [Required]
        [StringLength(400)]
        public string? Description { get; init; }

        [Required]
        public ISet<TagDTO>? Tags { get; init; }
        
        [Required]
        public ISet<UserDTO>? Supervisors { get; set; }
    }

    public record ProjectCreateDTO
    {
        [Required]
        public string Name { get; init; }

        [Required]
        [StringLength(400)]
        public string? Description { get; init; }

        [Required]
        public ISet<int>? TagIds { get; init; }

        [Required]
        public ISet<int>? UserIds { get; init; }
    }

    public record ProjectUpdateDTO : ProjectCreateDTO { }
}
