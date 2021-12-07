using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBank.Shared
{
    // TODO: Delete me.
    public class TagGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public bool RequiredInProject { get; set; } = false;
        public bool SupervisorCanAddTag { get; set; } = false;
        public int TagLimit { get; set; } = 0;
        public static IEnumerable<TagGroup> SampleTagGroups
        {
            get => new List<TagGroup>
            {
                new TagGroup
                {
                    Id = 1,
                    Name = "Programme",
                    Tags = Tag.SampleTagsProgrammes,
                },
                new TagGroup
                {
                    Id = 2,
                    Name = "Level",
                    Tags = Tag.SampleTagsLevels,
                },
                new TagGroup
                {
                    Id = 3,
                    Name = "Topics",
                    Tags = Tag.SampleTagsTopics,
                },
                new TagGroup
                {
                    Id = 4,
                    Name = "Semester",
                    Tags = Tag.SampleTagsSemesters,
                },
            };
        }
    }
}
