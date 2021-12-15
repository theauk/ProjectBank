namespace ProjectBank.Infrastructure.Tests;

public class UserRepositoryTests : RepoTests
{
    private readonly UserRepository _repository;

    public UserRepositoryTests() => _repository = new UserRepository(_context);
    
    // Create

    [Fact]
    public async Task CreateAsync_returns_Created_response()
    {
        var u = new UserCreateDTO
        {
            Name = "John Doe",
        };

        var response = await _repository.CreateAsync(u);
        
        Assert.Equal(Response.Created, response);
    }

    [Theory]
    [InlineData("John Doe")]
    [InlineData("Hilda Twinton")]
    [InlineData("Tony Bark")]
    [InlineData("The Janitor")]
    public async Task CreateAsync_adds_user_to_context(string name)
    {
        await _repository.CreateAsync(
            new UserCreateDTO {Name = name}
        );
        
        Assert.NotNull(_context.Users.FirstOrDefault(user => user.Name == name));
    }

    [Fact]
    public async Task Correct_user_count_after_addition()
    {
        var u = new UserCreateDTO
        {
            Name = "John Doe",
        };

        await _repository.CreateAsync(u);
        
        Assert.Equal(6, _context.Users.Count());
    }

    [Fact]
    public async Task cannot_create_invalid_user()
    {
        var u = new UserCreateDTO();

        try
        {
            await _repository.CreateAsync(u);
            throw new AccessViolationException("Should have throw exception here");
        }
        catch (Exception e)
        {
            Assert.Equal(typeof(DbUpdateException), e.GetType());
        }
    }
    
    // ReadAll

    [Fact]
    public async Task Gets_all_users_on_ReadAll()
    {
        var expected = new List<int> { 1, 2, 3, 4, 5, };
        
        var users = await _repository.ReadAllAsync();

        foreach (var id in expected)
            Assert.Contains(id, users.Select(u => u.Id));
    }
    
    // ReadAsync

    [Theory]
    [InlineData(1, "Marco")]
    [InlineData(2, "Birgit")]
    [InlineData(3, "Bjørn")]
    [InlineData(4, "Paolo")]
    [InlineData(5, "Rasmus")]
    public async Task Gets_correct_user_by_id(int id, string userName)
    {
        var result = await _repository.ReadAsync(id);
        if (result.IsNone)
            throw new Exception("Expected user does not exist.");
        
        var user = result.Value;
        
        Assert.Equal(userName, user.Name);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    public async Task Get_returns_null_on_non_existant_id(int id) => Assert.True((await _repository.ReadAsync(id)).IsNone);

    [Fact]
    public async Task Gets_active_users_correctly()
    {
        var users = (await _repository.ReadAllActiveAsync()).Select(u => u.Id);
        
        // Should include
        foreach (var uid in new List<int> { 1, 2, 3, 4, 5, })
            Assert.Contains(uid, users);
        
        // Should not include
        foreach (var uid in new List<int> { -1, 0, 6, 100, int.MaxValue })
            Assert.DoesNotContain(uid, users);
    }
}
