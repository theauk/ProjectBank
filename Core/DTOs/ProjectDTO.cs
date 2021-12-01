namespace Core
{
    public record ProjectDTO
    {
        public int Id { get; init; }

        [Required]
        public int Name { get; init; }

        [Required]
        [StringLength(400)]
        public string? Description { get; init; }

        [Required]
        public ISet<TagDTO>? TagDTOs { get; init; }
    }

    public record ProjectCreateDTO
    {
        [Required]
        public int Name { get; init; }

        [Required]
        [StringLength(400)]
        public string? Description { get; init; }

        [Required]
        public ISet<TagDTO>? TagDTOs { get; init; }
    }

    public record ProjectUpdateDTO : ProjectCreateDTO { }
}
