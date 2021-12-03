namespace Infrastructure
{
    public interface IProjectBankContext : IDisposable
    {
        DbSet<University> Universities { get; }
        DbSet<User> Users { get; }
        DbSet<Project> Projects { get; }
        DbSet<TagGroup> TagGroups { get; }
        DbSet<Tag> Tags { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
