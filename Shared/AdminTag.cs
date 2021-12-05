using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBank.Shared
{
    // TODO: Delete me.
    public class AdminTag
    {  
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsRequired { get; set; } = false;
        public bool Locked { get; set; } = false;
        public int MaxCount { get; set; } = 0;
        public static IEnumerable<AdminTag> SampleTagsTopics
        {
            get => new List<AdminTag>()
            {
                new AdminTag()
                {
                    Id = 1,
                    Name = "Number Theory",
                },
                new AdminTag()
                {
                    Id = 2,
                    Name = "Cryptography",
                },
                new AdminTag()
                {
                    Id = 3,
                    Name = "Set Theory",
                },
                new AdminTag()
                {
                    Id = 4,
                    Name = "RegEx's",
                },
                new AdminTag()
                {
                    Id = 5,
                    Name = "Automatas",
                },
                new AdminTag()
                {
                    Id = 6,
                    Name = "HTML Hell",
                },
                new AdminTag()
                {
                    Id = 7,
                    Name = "First Aid",
                },
                new AdminTag()
                {
                    Id = 8,
                    Name = "Ethics",
                },
                new AdminTag()
                {
                    Id = 9,
                    Name = "Rhetoric",
                },
                new AdminTag()
                {
                    Id = 10,
                    Name = "Logic",
                },
                new AdminTag()
                {
                    Id = 11,
                    Name = "Cardio-vascular",
                },
            };
        }
        public static IEnumerable<AdminTag> SampleTagsSemesters
        {
            get => new List<AdminTag>()
            {
                new AdminTag()
                {
                    Id = 12,
                    Name = "Spring 2022",
                },
                new AdminTag()
                {
                    Id = 13,
                    Name = "Autumn 2022",
                },
                new AdminTag()
                {
                    Id = 14,
                    Name = "Spring 2023",
                },
                new AdminTag()
                {
                    Id = 15,
                    Name = "Autumn 2023",
                },
                new AdminTag()
                {
                    Id = 16,
                    Name = "End of days",
                },
            };
        }
        public static IEnumerable<AdminTag> SampleTagsLevels
        {
            get => new List<AdminTag>()
            {
                new AdminTag()
                {
                    Id= 17,
                    Name = "Bachelor",
                },
                new AdminTag()
                {
                    Id= 18,
                    Name = "Master",
                },
                new AdminTag()
                {
                    Id= 19,
                    Name = "Ph.D",
                },
            };
        }
        public static IEnumerable<AdminTag> SampleTagsProgrammes
        {
            get => new List<AdminTag>()
            {
                new AdminTag()
                {
                    Id= 20,
                    Name = "Computer Science",
                },
                new AdminTag()
                {
                    Id= 21,
                    Name = "Software Development",
                },
                new AdminTag()
                {
                    Id= 22,
                    Name = "Mathematics",
                },
                new AdminTag()
                {
                    Id= 23,
                    Name = "Anatomy",
                },
                new AdminTag()
                {
                    Id= 24,
                    Name = "Philosophy",
                },
                new AdminTag()
                {
                    Id= 25,
                    Name = "Law",
                },
                new AdminTag()
                {
                    Id= 25,
                    Name = "Athletics",
                },
            };
        }
        public static IEnumerable<AdminTag> AllTags
        {
            get => SampleTagsTopics
                .Concat(SampleTagsSemesters)
                .Concat(SampleTagsLevels)
                .Concat(SampleTagsSemesters);
        }
    }
}
