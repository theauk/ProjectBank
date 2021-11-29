namespace Core
{
    public record ProjectDTO //DTO som bruges til at vise projekter i oversigten
    {
        public int Id { get; init; }

        [Required]
        public int Name { get; init; }

        [Required]
        [StringLength(400)] // Begrænsing af Description feltet, da vi alligevel aldrig kommer til at bruge den hele. Værd at overveje (i rapporten i hvertfald) om vi burde have en Summary i kombi med Description i stedet, eller om Description skal have en limit, .
        public string? Description { get; init; }

        [Required]
        public ISet<TagDTO>? TagDTOs { get; init; }
    }

    public record ProjectCreateDTO
    {
        [Required]
        public int Name { get; init; }

        [Required]
        [StringLength(400)] // Begrænsing af Description feltet, da vi alligevel aldrig kommer til at bruge den hele. Værd at overveje (i rapporten i hvertfald) om vi burde have en Summary i kombi med Description i stedet, eller om Description skal have en limit, .
        public string? Description { get; init; }

        [Required]
        public ISet<TagDTO>? TagDTOs { get; init; }
    }

    public record ProjectUpdateDTO : ProjectCreateDTO // Id bliver sendt videre i iRepo
    {

    }
}
