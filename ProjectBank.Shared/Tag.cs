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
                    Name = "Number Theory",
                },
                new Tag()
                {
                    Id = 2,
                    Name = "Cryptography",
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
                    Name = "Ethics",
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
                    Name = "Cardio-vascular",
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
        public static IEnumerable<Tag> SampleTagsLevels
        {
            get => new List<Tag>()
            {
                new Tag()
                {
                    Id= 17,
                    Name = "Bachelor",
                },
                new Tag()
                {
                    Id= 18,
                    Name = "Master",
                },
                new Tag()
                {
                    Id= 19,
                    Name = "Ph.D",
                },
            };
        }
        public static IEnumerable<Tag> SampleTagsProgrammes
        {
            get => new List<Tag>()
            {
                new Tag()
                {
                    Id= 20,
                    Name = "Computer Science",
                },
                new Tag()
                {
                    Id= 21,
                    Name = "Software Development",
                },
                new Tag()
                {
                    Id= 22,
                    Name = "Mathematics",
                },
                new Tag()
                {
                    Id= 23,
                    Name = "Anatomy",
                },
                new Tag()
                {
                    Id= 24,
                    Name = "Philosophy",
                },
                new Tag()
                {
                    Id= 25,
                    Name = "Law",
                },
                new Tag()
                {
                    Id= 26,
                    Name = "Athletics",
                },
            };
        }
        public static IEnumerable<Tag> AllTags => SampleTagsTopics
            .Concat(SampleTagsSemesters)
            .Concat(SampleTagsLevels)
            .Concat(SampleTagsProgrammes);
    }
}
