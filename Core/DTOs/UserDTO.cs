namespace Core
{
    public record UserDTO
    {
        public int Id {get; init;}
        
        [Required]
        public string? Name {get; init;}
    }

    public record UserCreateDTO 
    {
        [Required]
        public string? Name {get; init;}
    }
}
