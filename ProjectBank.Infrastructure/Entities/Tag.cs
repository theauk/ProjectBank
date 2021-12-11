namespace ProjectBank.Infrastructure.Entities
{
    public class Tag : IComparable<Tag>, IComparable

    {
        public int Id { get; set; }

        [Required] [StringLength(100)] public string Value { get; set; }

        public TagGroup TagGroup { get; set; }

        public ICollection<Project> Projects { get; set; } = new List<Project>();

        public int CompareTo(Tag tag) => string.Compare(Value, tag.Value);

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Tag otherTag = obj as Tag;
            if (otherTag != null)
                return CompareTo(otherTag);
            throw new ArgumentException("Object is not a Tag");
        }

    }
}
