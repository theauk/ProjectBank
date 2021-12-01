namespace Infrastructure
{
    public class ProjectBankContext : DbContext, IProjectBankContext
    {
        public DbSet<University> Universities => Set<University>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<TagGroup> TagGroups => Set<TagGroup>();
        public DbSet<Tag> Tags => Set<Tag>();

        public ProjectBankContext(DbContextOptions<ProjectBankContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<University>(e => {

            });            
        }

        public static void Seed(ProjectBankContext context)
        {
            
        }
    }
}
