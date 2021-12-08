namespace ProjectBank.Shared
{
    // TODO: Delete me.
    public class TagGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public bool IsRequired { get; set; } = false;
        public bool Locked { get; set; }
        public int MaxCount { get; set; } = 0;
        public static IEnumerable<TagGroup> SampleTagGroups =>
            new List<TagGroup>
            {
                new TagGroup
                {
                    Id = 1,
                    Name = "Programme",
                    Tags = Tag.SampleTagsProgrammes,
                    Locked = true,
                    IsRequired = true
                },
                new TagGroup
                {
                    Id = 2,
                    Name = "Level",
                    Tags = Tag.SampleTagsLevels,
                    Locked = true,
                    IsRequired = true
                },
                new TagGroup
                {
                    Id = 3,
                    Name = "Topics",
                    Tags = Tag.SampleTagsTopics,
                    IsRequired = true
                },
                new TagGroup
                {
                    Id = 4,
                    Name = "Semester",
                    Tags = Tag.SampleTagsSemesters,
                    Locked = true,
                },
            };
    }
}
