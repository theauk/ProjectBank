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
                e.HasIndex(e => e.Id)
                .IsUnique();
            });

            modelBuilder.Entity<User>(e => {
                e.HasIndex(e => e.Id)
                .IsUnique();
            });

            modelBuilder.Entity<Project>(e => {
                e.HasIndex(e => e.Id)
                .IsUnique();
            });

            modelBuilder.Entity<Tag>(e => {
                e.HasIndex(e => e.Id)
                .IsUnique();

                e.HasIndex(e => e.Value)
                .IsUnique();
            });

            modelBuilder.Entity<TagGroup>(e => {
                e.HasIndex(e => e.Id)
                .IsUnique();
            });
        }

        public async Task SeedAsync()
        {
            // Migrate pending migrations
            await Database.MigrateAsync();

            // Create universities
            // if (!await Universities.AnyAsync())
            // {
                
            // }

            // // Create users
            // if (!await Users.AnyAsync())
            // {

            // }

            // // Create Projects
            // if (!await Projects.AnyAsync())
            // {

            // }

            // // Create TagGroups
            // if (!await TagGroups.AnyAsync())
            // {

            // }

            // // Create Tags
            // if (!await Tags.AnyAsync())
            // {

            // }

            // await SaveChangesAsync();
        }
    }
}
