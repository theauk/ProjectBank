namespace ProjectBank.Infrastructure.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Value { get; set; }

        public Tag(string value)
        {
            Value = value;
        }
    }
}
