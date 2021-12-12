namespace ProjectBank.Infrastructure
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
                e.HasKey(university => university.DomainName);
                e.HasIndex(university => university.DomainName)
                .IsUnique();
            });

            modelBuilder.Entity<User>(e => {
                e.HasIndex(user => user.Id)
                .IsUnique();
            });

            modelBuilder.Entity<Project>(e => {
                e.HasIndex(project => project.Id)
                .IsUnique();
            });

            modelBuilder.Entity<Tag>(e => {
                e.HasIndex(tag => tag.Id)
                .IsUnique();

                e.HasIndex(tag => tag.Value)
                .IsUnique();
            });

            modelBuilder.Entity<TagGroup>(e => {
                e.HasIndex(tagGroup => tagGroup.Id)
                .IsUnique();
            });
        }
    }
}
