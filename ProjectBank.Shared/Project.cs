using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBank.Shared
{
    // TODO: Delete me.
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public static IEnumerable<Project> SampleProjects
        {
            get => new List<Project>()
            {
                new Project
                {
                    Id = 1,
                    Name = "Invent Polio",
                    Tags = new List<string>
                    {
                        "Science",
                        "Genetics",
                    },
                },
                new Project
                {
                    Id = 2,
                    Name = "Cure Polio",
                    Tags = new List<string>
                    {
                        "Science",
                        "Ethics",
                    },
                },
            };
        }

        public static Project GetProject(int id)
        {
            return SampleProjects.Where(p => p.Id == id).First();;
        }

        public string TagString()
        {
            return String.Join(", ", Tags);
        }
    }
}
