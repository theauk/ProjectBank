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
        
        // Users
        var marco  = new User { Name = "Marco" , University = itu, Email = "marco@itu.dk",  Role = Role.Admin };
        var birgit = new User { Name = "Birgit", University = itu, Email = "birgit@itu.dk", Role = Role.Admin };
        var bjorn  = new User { Name = "Bjørn" , University = itu, Email = "bjorn@itu.dk",  Role = Role.Admin };
        var paolo  = new User { Name = "Paolo" , University = itu, Email = "paolo@itu.dk",  Role = Role.Admin };
        var rasmus = new User { Name = "Rasmus", University = itu, Email = "rasmus@itu.dk", Role = Role.Admin };
        var jens   = new User { Name = "Jens",   University = itu, Email = "jens@itu.dk",   Role = Role.Supervisor };
        var ib     = new User { Name = "Ib",     University = itu, Email = "ib@itu.dk",     Role = Role.Student };
        
        _context.AddRange(new List<User> {marco, birgit, bjorn, paolo, rasmus, jens, ib});
        
        // Tags
        var semesterTags = new List<Tag>
        {
            new() {Value = "Spring 2022"},
            new() {Value = "Autumn 2022"},
            new() {Value = "Spring 2023"},
            new() {Value = "Autumn 2023"},
            new() {Value = "Spring 2024"},
            new() {Value = "Autumn 2024"},
        };
        var plangTags = new List<Tag>
        {
            new() {Value = ".NET"},
            new() {Value = "Python"},
            new() {Value = "Rust"},
            new() {Value = "C"},
            new() {Value = "C++"},
            new() {Value = "Java (yuck)"},
            new() {Value = "JavaScript (yuckier)"},
        };
        var levelTags = new List<Tag>
        {
            new() {Value = "Bachelor"},
            new() {Value = "Master"},
            new() {Value = "PhD"},
        };
        
        // TagGroups
        var semester = new TagGroup
        {
            Name = "Semester",
            Tags = semesterTags,
            RequiredInProject = true,
            SupervisorCanAddTag = false,
            TagLimit = 2,
            University = itu
        };
        var plangs = new TagGroup
        {
            Name = "Programming Language",
            Tags = plangTags,
            RequiredInProject = false,
            SupervisorCanAddTag = true,
            TagLimit = 3,
            University = itu
        };
        var levels = new TagGroup
        {
            Name = "Level",
            Tags = levelTags,
            RequiredInProject = true,
            SupervisorCanAddTag = false,
            TagLimit = null,
            University = itu
        };
        
        _context.AddRange(new List<TagGroup> {semester, plangs, levels});
        
        // Projects

        var projects = new List<Project>
        {
            new()
            {
                Name = "Invent AI",
                Description = "Your task, should you choose to accept it, is to threaten the existence of life on earth.",
                Supervisors = new List<User> {rasmus, bjorn},
                Tags = new List<Tag> {semesterTags[0], plangTags[3], plangTags[4], levelTags[2]},
                University = itu
            },
            new()
            {
                Name = "Project Bank",
                Description = "Replace our current system.",
                Supervisors = new List<User> {rasmus, paolo},
                Tags = new List<Tag> {semesterTags[0], plangTags[0], levelTags[0]},
                University = itu
            },
            new()
            {
                Name = "Make an app!",
                Description = "Like a dating app, or something. Just something we can sell for a lot of money.",
                Supervisors = new List<User> {birgit, marco},
                Tags = new List<Tag> {semesterTags[1], semesterTags[2], plangTags[6], levelTags[0]},
                University = itu
            },
        };
        
        _context.AddRange(projects);

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