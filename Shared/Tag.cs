using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBank.Shared
{
    // TODO: Delete me.
    public class Tag
    {  
        public int Id { get; set; }
        public string Name { get; set; }
        public static IEnumerable<Tag> SampleTagsTopics
        {
            get => new List<Tag>()
            {
                new Tag()
                {
                    Id = 1,
                    Name = "Math",
                },
                new Tag()
                {
                    Id = 2,
                    Name = "Crypto",
                },
                new Tag()
                {
                    Id = 3,
                    Name = "Set Theory",
                },
                new Tag()
                {
                    Id = 4,
                    Name = "RegEx's",
                },
                new Tag()
                {
                    Id = 5,
                    Name = "Automatas",
                },
                new Tag()
                {
                    Id = 6,
                    Name = "HTML Hell",
                },
                new Tag()
                {
                    Id = 7,
                    Name = "First Aid",
                },
                new Tag()
                {
                    Id = 8,
                    Name = "Philosophy",
                },
                new Tag()
                {
                    Id = 9,
                    Name = "Rhetoric",
                },
                new Tag()
                {
                    Id = 10,
                    Name = "Logic",
                },
                new Tag()
                {
                    Id = 11,
                    Name = "Anatomy",
                },
            };
        }
        public static IEnumerable<Tag> SampleTagsSemesters
        {
            get => new List<Tag>()
            {
                new Tag()
                {
                    Id = 12,
                    Name = "Spring 2022",
                },
                new Tag()
                {
                    Id = 13,
                    Name = "Autumn 2022",
                },
                new Tag()
                {
                    Id = 14,
                    Name = "Spring 2023",
                },
                new Tag()
                {
                    Id = 15,
                    Name = "Autumn 2023",
                },
                new Tag()
                {
                    Id = 16,
                    Name = "End of days",
                },
            };
        }
    }
}
