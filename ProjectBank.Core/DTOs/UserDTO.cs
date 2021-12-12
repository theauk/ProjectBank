namespace ProjectBank.Core.DTOs
{
    public record UserDTO
    {
        public int Id { get; init; }

        [Required] 
        public string Name { get; init; }
        
        [Required]
        public string Email { get; set; }
    }

    public record UserCreateDTO
    {
        [Required] 
        public string Name { get; set; }
        
        [Required]
        public string Email { get; set; }
    }
}