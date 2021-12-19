namespace ProjectBank.Infrastructure.Tests;

public class RepoTests : IDisposable
{
    protected readonly ProjectBankContext _context;
    private bool _disposedValue;

    public RepoTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);

        _context = new ProjectBankContext(builder.Options);
        _context.Database.EnsureCreated();

        // University
        var itu = new University()
        {
            DomainName = "itu.dk",
        };

        var ruc = new University()
        {
            DomainName = "ruc.dk"
        };

        _context.Universities.AddRange(itu, ruc);

        // Users
        var marco = new User { Id = 1, Name = "Marco", University = itu, Email = "marco@itu.dk", Role = Role.Admin };
        var birgit = new User { Id = 2, Name = "Birgit", University = itu, Email = "birgit@itu.dk", Role = Role.Admin };
        var bjorn = new User { Id = 3, Name = "Bjørn", University = itu, Email = "bjorn@itu.dk", Role = Role.Admin };
        var paolo = new User { Id = 4, Name = "Paolo", University = itu, Email = "paolo@itu.dk", Role = Role.Admin };
        var rasmus = new User { Id = 5, Name = "Rasmus", University = itu, Email = "rasmus@itu.dk", Role = Role.Admin };
        var jens = new User { Id = 6, Name = "Jens", University = itu, Email = "jens@itu.dk", Role = Role.Supervisor };
        var ib = new User { Id = 7, Name = "Ib", University = itu, Email = "ib@itu.dk", Role = Role.Student };
        var heidi = new User { Id = 8, Name = "Heidi", University = ruc, Email = "heidi@ruc.dk", Role = Role.Supervisor };

        _context.Users.AddRange(marco, birgit, bjorn, paolo, rasmus, jens, ib, heidi);

        // Tags
        // Semester
        var spring22 = new Tag { Id = 1, Value = "Spring 2022" };
        var autumn22 = new Tag { Id = 2, Value = "Autumn 2022" };
        var spring23 = new Tag { Id = 3, Value = "Spring 2023" };
        var autumn23 = new Tag { Id = 4, Value = "Autumn 2023" };
        var spring24 = new Tag { Id = 5, Value = "Spring 2024" };
        var autumn24 = new Tag { Id = 6, Value = "Autumn 2024" };

        // Programming language
        var dotnet = new Tag { Id = 7, Value = ".NET" };
        var python = new Tag { Id = 8, Value = "Python" };
        var rust = new Tag { Id = 9, Value = "Rust" };
        var c = new Tag { Id = 10, Value = "C" };
        var cplusplus = new Tag { Id = 11, Value = "C++" };
        var java = new Tag { Id = 12, Value = "Java (yuck)" };
        var javascript = new Tag { Id = 13, Value = "JavaScript (yuckier)" };

        // Level
        var bsc = new Tag { Id = 14, Value = "Bachelor" };
        var msc = new Tag { Id = 15, Value = "Master" };
        var pdh = new Tag { Id = 16, Value = "PhD" };

        // TagGroups
        var semester = new TagGroup
        {
            Id = 1,
            Name = "Semester",
            Tags = new List<Tag> { spring22, autumn22, spring23, autumn23, spring24, autumn24 },
            RequiredInProject = true,
            SupervisorCanAddTag = false,
            TagLimit = 2,
            University = itu
        };
        var plangs = new TagGroup
        {
            Id = 2,
            Name = "Programming Language",
            Tags = new List<Tag> { dotnet, python, rust, c, cplusplus, java, javascript },
            RequiredInProject = false,
            SupervisorCanAddTag = true,
            TagLimit = 3,
            University = itu
        };
        var levels = new TagGroup
        {
            Id = 3,
            Name = "Level",
            Tags = new List<Tag> { bsc, msc, pdh },
            RequiredInProject = true,
            SupervisorCanAddTag = false,
            TagLimit = null,
            University = itu
        };

        var language = new TagGroup
        {
            Id = 4,
            Name = "Language",
            Tags = new List<Tag> { },
            RequiredInProject = false,
            SupervisorCanAddTag = false,
            TagLimit = null,
            University = ruc
        };

        _context.TagGroups.AddRange(semester, plangs, levels, language);

        // Projects
        var projects = new HashSet<Project>
        {
            new()
            {
                Id = 1,
                Name = "Invent AI",
                Description = "Your task, should you choose to accept it, is to threaten the existence of life on earth.",
                Supervisors = new HashSet<User> {rasmus, bjorn},
                Tags = new HashSet<Tag> { spring22, c, cplusplus, pdh},
                University = itu
            },
            new()
            {
                Id = 2,
                Name = "Project Bank",
                Description = "Replace our current system.",
                Supervisors = new HashSet<User> {rasmus, paolo},
                Tags = new HashSet<Tag> { spring22, dotnet, bsc},
                University = itu
            },
            new()
            {
                Id = 3,
                Name = "Make an app!",
                Description = "Like a dating app, or something. Just something we can sell for a lot of money.",
                Supervisors = new HashSet<User> {birgit, marco},
                Tags = new HashSet<Tag> {autumn22, spring23, javascript, bsc},
                University = itu
            },
            new()
            {
                Id = 4,
                Name = "Idk",
                Description = "Out of ideas for projects.",
                Supervisors = new HashSet<User> { heidi },
                Tags = new List<Tag> {},
                University = ruc
            },
        };

        _context.Projects.AddRange(projects);

        _context.SaveChanges();
    }

    [Fact]
    public void Repo_base_constructor_works()
    {
        Assert.True(true);
    }

    protected virtual void Dispose(bool disposing) => _disposedValue = _disposedValue || disposing;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}