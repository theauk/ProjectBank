namespace ProjectBank.Infrastructure.Tests.Repositories;

public class ProjectRepositoryTests : RepoTests
{
    private readonly ProjectRepository _repository;

    public ProjectRepositoryTests() => _repository = new ProjectRepository(_context);

    [Fact]
    public async Task CreateAsync_creates_new_project_with_generated_id()
    {
        var project = new ProjectCreateDTO
        {
            Name = "Bachelor Project",
            Description = "The final project of SWU.",
            OwnerEmail = "paolo@itu.dk",
            ExistingTagIds = new HashSet<int> { 1 },
            UserIds = new HashSet<int> { 4, 5 }
        };

        var response = await _repository.CreateAsync(project);
        var actualProject = (await _repository.ReadAsync(4)).Value;

        Assert.Equal(Response.Created, response);
        Assert.Equal(4, actualProject?.Id);
        Assert.Equal("Bachelor Project", actualProject?.Name);
        Assert.Equal("The final project of SWU.", actualProject?.Description);
        foreach (var tagId in new List<int> { 1, })
            Assert.Contains(tagId, actualProject?.Tags.Select(t => t.Id));
        foreach (var supeId in new List<int> { 4, 5, })
            Assert.Contains(supeId, actualProject?.Supervisors.Select(s => s.Id));
    }

    [Fact]
    public async Task CreateAsync_given_invalid_Project_Owner_email_returns_BadRequest()
    {
        var project = new ProjectCreateDTO
        {
            Name = "Bachelor Project",
            Description = "The final project of SWU.",
            OwnerEmail = "test@itu.dk",
            ExistingTagIds = new HashSet<int>() { 1 },
            UserIds = new HashSet<int>() { 4, 6 }
        };

        var actual = await _repository.CreateAsync(project);

        Assert.Equal(Response.BadRequest, actual);
    }

    [Fact]
    public async Task CreateAsync_given_non_existing_User_Id_returns_BadRequest()
    {
        var project = new ProjectCreateDTO
        {
            Name = "Bachelor Project",
            Description = "The final project of SWU.",
            OwnerEmail = "paolo@itu.dk",
            ExistingTagIds = new HashSet<int>() { 1 },
            UserIds = new HashSet<int>() { 4, 43 }
        };

        var actual = await _repository.CreateAsync(project);

        Assert.Equal(Response.BadRequest, actual);
    }

    [Fact]
    public async Task DeleteAsync_given_existing_id_deletes()
    {
        var actual = await _repository.DeleteAsync(3);

        Assert.Equal(Response.Deleted, actual);
        Assert.Null(await _context.Projects.FindAsync(3));
    }

    [Fact]
    public async Task DeleteAsync_given_non_existing_id_returns_NotFound()
    {
        var actual = await _repository.DeleteAsync(0);
        Assert.Equal(Response.NotFound, actual);
    }

    [Fact]
    public async Task ReadAsync_given_id_exists_returns_Project()
    {
        var project = (await _repository.ReadAsync(3)).Value;

        Assert.NotNull(project);
        Assert.Equal(3, project?.Id);
        Assert.Equal("Make an app!", project?.Name);
        Assert.Equal("Like a dating app, or something. Just something we can sell for a lot of money.", project?.Description);
        foreach (var tagId in new List<int> { 2, 3, 13, 14, })
            Assert.Contains(tagId, project?.Tags.Select(t => t.Id));
        foreach (var supeId in new List<int> { 1, 2, })
            Assert.Contains(supeId, project?.Supervisors.Select(s => s.Id));
    }

    [Fact]
    public async Task ReadAsync_given_non_existing_id_returns_Null()
    {
        var actual = (await _repository.ReadAsync(33)).Value;
        Assert.Null(actual);
    }

    [Fact]
    public async Task ReadAllAsync_returns_all_projects() => Assert.Equal(new List<int> { 1, 2, 3, },
        (await _repository.ReadAllAsync()).Select(p => p.Id));

    [Fact]
    public async Task UpdateAsync_updates_existing_character()
    {
        var project = new ProjectUpdateDTO
        {
            Name = "Extreme Math Project",
            Description = "Prove even harder stuff.",
            ExistingTagIds = new HashSet<int>(),
            UserIds = new HashSet<int>() { 1, 2 }
        };

        var response = await _repository.UpdateAsync(1, project);

        Assert.Equal(Response.Updated, response);

        var mathProject = (await _repository.ReadAsync(1)).Value;

        Assert.NotNull(mathProject);
        Assert.Empty(mathProject?.Tags);
    }

    [Fact]
    public async Task UpdateAsync_given_non_existing_id_returns_NotFound()
    {
        var project = new ProjectUpdateDTO
        {
            Name = "Extreme Math Project",
            Description = "Prove even harder stuff.",
            ExistingTagIds = new HashSet<int>() { 1 },
            UserIds = new HashSet<int>() { 1, 2 }
        };

        var actual = await _repository.UpdateAsync(33, project);

        Assert.Equal(Response.NotFound, actual);
    }

    [Fact]
    public async Task UpdateAsync_given_non_existing_user_id_returns_BadRequest()
    {
        var project = new ProjectUpdateDTO
        {
            Name = "Extreme Math Project",
            Description = "Prove even harder stuff.",
            ExistingTagIds = new HashSet<int>() { 1 },
            UserIds = new HashSet<int>() { 1, 8 }
        };

        var actual = await _repository.UpdateAsync(1, project);

        Assert.Equal(Response.BadRequest, actual);
    }
}
