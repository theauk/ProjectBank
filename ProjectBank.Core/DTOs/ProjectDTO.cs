namespace ProjectBank.Core.DTOs
{
    public record ProjectDTO
    {
        public int Id { get; init; }

        [Required]
        public string? Name { get; init; }

        [Required]
        [StringLength(400)]
        public string? Description { get; init; }

        [Required]
        public ISet<TagDTO>? TagDTOs { get; init; }
    }

    public record ProjectCreateDTO
    {
        [Required]
        public string? Name { get; init; }

        [Required]
        [StringLength(400)]
        public string? Description { get; init; }

        [Required]
        public ISet<int>? TagIds { get; init; }
    }

    public record ProjectUpdateDTO : ProjectCreateDTO { }
}
