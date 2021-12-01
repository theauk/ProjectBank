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
        public static IEnumerable<TagGroup> SampleTagGroups
        {
            get => new List<TagGroup>
            {
                new TagGroup
                {
                    Id = 1,
                    Name = "Topics",
                    Tags = Tag.SampleTagsTopics,
                },
                new TagGroup
                {
                    Id = 2,
                    Name = "Semester",
                    Tags = Tag.SampleTagsSemesters,
                },
            };
        }
    }
}
