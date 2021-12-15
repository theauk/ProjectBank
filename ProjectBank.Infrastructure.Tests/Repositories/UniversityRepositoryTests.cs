namespace ProjectBank.Infrastructure.Tests.Repositories;

public class UniversityRepositoryTests : RepoTests
{
    private readonly UniversityRepository _repository;

    public UniversityRepositoryTests() => _repository = new UniversityRepository(_context);

    [Fact]
    public async Task CreateAsync_creates_University_and_returns_Created()
    {
        var university = new UniversityCreateDTO
        {
            DomainName = "dtu.dk"
        };

        var response = await _repository.CreateAsync(university);
        var created = _context.Universities.Find("dtu.dk")?.ToDTO();
        
        Assert.Equal(Response.Created, response);
        Assert.Equal("dtu.dk", created?.DomainName);
        Assert.True(created?.Users.SetEquals(new HashSet<UserDTO>()));
        Assert.True(created?.Projects.SetEquals(new HashSet<ProjectDTO>()));
        Assert.True(created?.TagGroups.SetEquals(new HashSet<TagGroupDTO>()));
    }

    [Fact]
    public async Task CreateAsync_given_already_taken_domain_returns_Conflict()
    {
        var university = new UniversityCreateDTO
        {
            DomainName = "itu.dk"
        };

        var response = await _repository.CreateAsync(university);
        
        Assert.Equal(Response.Conflict, response);
    }

    // [Fact]
    // public async Task UpdateASync_given_existing_domain_updates_University_and_returns_Updated()
    // {

    // }

    [Fact]
    public async Task UpdateAsync_given_non_existing_domain_returns_NotFound()
    {
        var uniUpdate = new UniversityUpdateDTO
        {
            DomainName = "dtu.dk",
        };
        var response = await _repository.UpdateAsync("dtu.dk", uniUpdate);
        Assert.Equal(Response.NotFound, response);
    }

    [Fact]
    public async Task DeleteAsync_given_existing_domain_deletes_University_and_returns_Deleted()
    {
        var response = await _repository.DeleteAsync("itu.dk");
        
        Assert.Equal(Response.Deleted, response);
        Assert.Null(_context.Universities.Find("itu.dk"));
    }

    [Fact]
    public async Task DeleteAsync_given_non_existing_domain_returns_NotFound()
    {
        var response = await _repository.DeleteAsync("dtu.dk");
        Assert.Equal(Response.NotFound, response);
    }

    [Fact]
    public async Task ReadAsync_given_existing_domain_returns_University()
    {
        var actual = (await _repository.ReadAsync("itu.dk")).Value;
        Assert.Equal("itu.dk", actual?.DomainName);
        Assert.Collection(actual?.Users, 
            user => Assert.Equal(new UserDTO { Id = 1, Name = "Marco", Email = "marco@itu.dk", Role = Role.Admin }, user),
            user => Assert.Equal(new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk", Role = Role.Admin }, user),
            user => Assert.Equal(new UserDTO { Id = 3, Name = "Bjørn", Email = "bjorn@itu.dk", Role = Role.Admin }, user),
            user => Assert.Equal(new UserDTO { Id = 4, Name = "Paolo", Email = "paolo@itu.dk", Role = Role.Admin }, user),
            user => Assert.Equal(new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin }, user),
            user => Assert.Equal(new UserDTO { Id = 6, Name = "Jens", Email = "jens@itu.dk", Role = Role.Supervisor }, user)
        );
    }

    [Fact]
    public async Task ReadAsync_given_non_existing_domain_returns_Null()
    {
        var actual = await _repository.ReadAsync("dtu.dk");
        Assert.True(actual.IsNone);
        Assert.Null(actual.Value);
    }
}
