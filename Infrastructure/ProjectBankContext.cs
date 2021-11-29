using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ProjectBankContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TagGroup> TagGroups { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public ProjectBankContext(DbContextOptions<ProjectBankContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
