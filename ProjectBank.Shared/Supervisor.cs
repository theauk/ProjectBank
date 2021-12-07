using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBank.Shared
{
    // TODO: Delete me.
    public class Supervisor
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static IEnumerable<Supervisor> SampleSupervisors => new List<Supervisor>()
        {
            new Supervisor()
            {
                Id = 1,
                Name = "Gustav",
            },
            new Supervisor()
            {
                Id = 2,
                Name = "Joanna",
            },
            new Supervisor()
            {
                Id = 3,
                Name = "Mai",
            },
            new Supervisor()
            {
                Id = 4,
                Name = "Thea",
            },
            new Supervisor()
            {
                Id = 5,
                Name = "Oliver",
            },
            new Supervisor()
            {
                Id = 6,
                Name = "Viktor",
            },
        };
    }
}
