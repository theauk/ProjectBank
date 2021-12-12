namespace ProjectBank.Infrastructure.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public University University { get; set; }

        public ICollection<Project> Projects { get; set; } = new HashSet<Project>(); // todo: this is never used???
        
        public string Email { get; set; }

        public UserDTO ToDTO()
        {
            return new UserDTO
            {
                Id = Id,
                Email = Email,
                Name = Name,
            };
        }
    }
}
