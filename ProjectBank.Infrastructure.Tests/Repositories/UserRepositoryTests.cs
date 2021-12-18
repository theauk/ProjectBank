namespace ProjectBank.Infrastructure.Tests.Repositories;

public class UserRepositoryTests : RepoTests
{
    private readonly UserRepository _repository;

    public UserRepositoryTests() => _repository = new UserRepository(_context);

    [Fact]
    public async Task CreateAsync_creates_User_and_returns_Created()
    {
        var user = new UserCreateDTO
        {
            Name = "John Doe",
            Email = "doe@itu.dk",
            Role = Role.Student
        };

        var response = await _repository.CreateAsync(user);
        var actual = _context.Users.FirstOrDefault(u => u.Email == "doe@itu.dk")?.ToDTO();

        Assert.Equal(Response.Created, response);
        Assert.Equal(new UserDTO { Id = 9, Name = "John Doe", Email = "doe@itu.dk", Role = Role.Student }, actual);
    }

    [Fact]
    public async Task CreateAsync_given_non_existing_university_returns_BadRequest()
    {
        var user = new UserCreateDTO
        {
            Name = "John Doe",
            Email = "doe@dtu.dk",
            Role = Role.Student
        };

        var response = await _repository.CreateAsync(user);

        Assert.Equal(Response.BadRequest, response);
    }

    [Theory]
    [InlineData("John Doe", "doe@itu.dk", Role.Student)]
    [InlineData("Hilda Twinton", "twin@itu.dk", Role.Student)]
    [InlineData("Tony Bark", "bark@itu.dk", Role.Student)]
    [InlineData("The Janitor", "jani@itu.dk", Role.Supervisor)]
    public async Task CreateAsync_adds_user_to_context(string name, string email, Role role)
    {
        await _repository.CreateAsync(
            new UserCreateDTO
            {
                Name = name,
                Email = email,
                Role = role
            }
        );

        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        Assert.NotNull(user);
    }

    [Fact]
    public async Task Adding_user_to_context_increases_max_number_of_users()
    {
        var user = new UserCreateDTO
        {
            Name = "John Doe",
            Email = "doe@itu.dk",
            Role = Role.Student
        };

        await _repository.CreateAsync(user);

        Assert.Equal(9, _context.Users.Count());
    }

    [Fact]
    public async Task ReadAllAsync_returns_all_Users_across_all_Universities()
    {
        var users = await _repository.ReadAllAsync();

        Assert.Collection(users,
            user => Assert.Equal(new UserDTO { Id = 1, Name = "Marco", Email = "marco@itu.dk", Role = Role.Admin }, user),
            user => Assert.Equal(new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk", Role = Role.Admin }, user),
            user => Assert.Equal(new UserDTO { Id = 3, Name = "Bjørn", Email = "bjorn@itu.dk", Role = Role.Admin }, user),
            user => Assert.Equal(new UserDTO { Id = 4, Name = "Paolo", Email = "paolo@itu.dk", Role = Role.Admin }, user),
            user => Assert.Equal(new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin }, user),
            user => Assert.Equal(new UserDTO { Id = 6, Name = "Jens", Email = "jens@itu.dk", Role = Role.Supervisor }, user),
            user => Assert.Equal(new UserDTO { Id = 7, Name = "Ib", Email = "ib@itu.dk", Role = Role.Student }, user),
            user => Assert.Equal(new UserDTO { Id = 8, Name = "Heidi", Email = "heidi@ruc.dk", Role = Role.Supervisor }, user)
        );
    }

    [Fact]
    public async Task ReadAllActiveAsync_returns_all_Users_with_minimum_1_Project()
    {
        var users = await _repository.ReadAllActiveAsync("test@itu.dk");

        Assert.Collection(users,
            user => Assert.Equal(new UserDTO { Id = 1, Name = "Marco", Email = "marco@itu.dk", Role = Role.Admin }, user),
            user => Assert.Equal(new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk", Role = Role.Admin }, user),
            user => Assert.Equal(new UserDTO { Id = 3, Name = "Bjørn", Email = "bjorn@itu.dk", Role = Role.Admin }, user),
            user => Assert.Equal(new UserDTO { Id = 4, Name = "Paolo", Email = "paolo@itu.dk", Role = Role.Admin }, user),
            user => Assert.Equal(new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin }, user),
            user => Assert.Equal(new UserDTO { Id = 8, Name = "Heidi", Email = "heidi@ruc.dk", Role = Role.Supervisor }, user)
        );
    }

    [Theory]
    [MemberData(nameof(GetUsersWithRoles))]
    public async Task ReadAllByRole_returns_all_Users_with_role(IList<string> roles, IReadOnlyCollection<UserDTO> expected)
    {
        var users = await _repository.ReadAllByRoleAsync("test@itu.dk", roles);

        Assert.Equal(expected, users);
    }

    [Fact]
    public async Task ReadAsync_given_existing_id_returns_User()
    {
        var user = (await _repository.ReadAsync(1)).Value;
        Assert.Equal(new UserDTO { Id = 1, Name = "Marco", Email = "marco@itu.dk", Role = Role.Admin }, user);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    public async Task ReadAsync_given_non_existing_id_returns_Null(int id)
    {
        var user = (await _repository.ReadAsync(id)).Value;
        Assert.Null(user);
    }

    [Fact]
    public async Task ReadAsync_given_existing_email_returns_User()
    {
        var user = (await _repository.ReadAsync("marco@itu.dk")).Value;
        Assert.Equal(new UserDTO { Id = 1, Name = "Marco", Email = "marco@itu.dk", Role = Role.Admin }, user);
    }

    [Theory]
    [InlineData("test@itu.dk")]
    [InlineData("NotAnEmail")]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ReadAsync_given_non_existing_email_returns_Null(string email)
    {
        var user = (await _repository.ReadAsync(email)).Value;
        Assert.Null(user);
    }

    [Fact]
    public async Task Gets_all_users_on_ReadAll()
    {
        var expected = new List<int> { 1, 2, 3, 4, 5, };

        var users = await _repository.ReadAllAsync();

        foreach (var id in expected)
            Assert.Contains(id, users.Select(u => u.Id));
    }

    [Theory]
    [InlineData(1, "Marco")]
    [InlineData(2, "Birgit")]
    [InlineData(3, "Bjørn")]
    [InlineData(4, "Paolo")]
    [InlineData(5, "Rasmus")]
    public async Task ReadAsync_correct_user_by_id(int id, string userName)
    {
        var result = await _repository.ReadAsync(id);
        if (result.IsNone)
            throw new Exception("Expected user does not exist.");

        var user = result.Value;

        Assert.Equal(userName, user?.Name);
    }

    [Fact]
    public async Task Gets_active_users_correctly()
    {
        var users = (await _repository.ReadAllActiveAsync("test@itu.dk")).Select(u => u.Id);

        // Should include
        foreach (var uid in new List<int> { 1, 2, 3, 4, 5, })
            Assert.Contains(uid, users);

        // Should not include
        foreach (var uid in new List<int> { -1, 0, 6, 100, int.MaxValue })
            Assert.DoesNotContain(uid, users);
    }

    public static IEnumerable<object[]> GetUsersWithRoles()
    {
        // No input returns all
        yield return new object[]
        {
            new List<String>(),
            new List<UserDTO>()
            {
                new UserDTO { Id = 1, Name = "Marco",  Email = "marco@itu.dk",  Role = Role.Admin },
                new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk", Role = Role.Admin },
                new UserDTO { Id = 3, Name = "Bjørn" , Email = "bjorn@itu.dk",  Role = Role.Admin },
                new UserDTO { Id = 4, Name = "Paolo" , Email = "paolo@itu.dk",  Role = Role.Admin },
                new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin },
                new UserDTO { Id = 6, Name = "Jens", Email = "jens@itu.dk", Role = Role.Supervisor },
                new UserDTO { Id = 7, Name = "Ib", Email = "ib@itu.dk", Role = Role.Student }
            }.AsReadOnly()
        };

        // Only Admins
        yield return new object[]
        {
            new List<string>() { "Admin" },
            new List<UserDTO>()
            {
                new UserDTO { Id = 1, Name = "Marco",  Email = "marco@itu.dk",  Role = Role.Admin },
                new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk", Role = Role.Admin },
                new UserDTO { Id = 3, Name = "Bjørn" , Email = "bjorn@itu.dk",  Role = Role.Admin },
                new UserDTO { Id = 4, Name = "Paolo" , Email = "paolo@itu.dk",  Role = Role.Admin },
                new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin }
            }.AsReadOnly()
        };

        // Only Supervisors
        yield return new object[]
        {
            new List<string>() { "Supervisor" },
            new List<UserDTO>()
            {
                new UserDTO { Id = 6, Name = "Jens", Email = "jens@itu.dk", Role = Role.Supervisor }
            }.AsReadOnly()
        };

        // Only Students
        yield return new object[]
        {
            new List<string>() { "Student" },
            new List<UserDTO>()
            {
                new UserDTO { Id = 7, Name = "Ib", Email = "ib@itu.dk", Role = Role.Student }
            }.AsReadOnly()
        };

        // Admin, supervisor
        yield return new object[]
        {
            new List<string>() { "Admin", "Supervisor" },
            new List<UserDTO>()
            {
                new UserDTO { Id = 1, Name = "Marco",  Email = "marco@itu.dk",  Role = Role.Admin },
                new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk", Role = Role.Admin },
                new UserDTO { Id = 3, Name = "Bjørn" , Email = "bjorn@itu.dk",  Role = Role.Admin },
                new UserDTO { Id = 4, Name = "Paolo" , Email = "paolo@itu.dk",  Role = Role.Admin },
                new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin },
                new UserDTO { Id = 6, Name = "Jens", Email = "jens@itu.dk", Role = Role.Supervisor }
            }.AsReadOnly()
        };

        // Admin, Student
        yield return new object[]
        {
            new List<string>() { "Admin", "Student" },
            new List<UserDTO>()
            {
                new UserDTO { Id = 1, Name = "Marco",  Email = "marco@itu.dk",  Role = Role.Admin },
                new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk", Role = Role.Admin },
                new UserDTO { Id = 3, Name = "Bjørn" , Email = "bjorn@itu.dk",  Role = Role.Admin },
                new UserDTO { Id = 4, Name = "Paolo" , Email = "paolo@itu.dk",  Role = Role.Admin },
                new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin },
                new UserDTO { Id = 7, Name = "Ib", Email = "ib@itu.dk", Role = Role.Student }
            }.AsReadOnly()
        };

        // Supervisor, Student
        yield return new object[]
        {
            new List<string>() { "Supervisor", "Student" },
            new List<UserDTO>()
            {
                new UserDTO { Id = 6, Name = "Jens", Email = "jens@itu.dk", Role = Role.Supervisor },
                new UserDTO { Id = 7, Name = "Ib", Email = "ib@itu.dk", Role = Role.Student }
            }.AsReadOnly()
        };

        // All roles
        yield return new object[]
        {
            new List<string>() { "Admin", "Supervisor", "Student" },
            new List<UserDTO>()
            {
                new UserDTO { Id = 1, Name = "Marco",  Email = "marco@itu.dk",  Role = Role.Admin },
                new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk", Role = Role.Admin },
                new UserDTO { Id = 3, Name = "Bjørn" , Email = "bjorn@itu.dk",  Role = Role.Admin },
                new UserDTO { Id = 4, Name = "Paolo" , Email = "paolo@itu.dk",  Role = Role.Admin },
                new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin },
                new UserDTO { Id = 6, Name = "Jens", Email = "jens@itu.dk", Role = Role.Supervisor },
                new UserDTO { Id = 7, Name = "Ib", Email = "ib@itu.dk", Role = Role.Student }
            }.AsReadOnly()
        };

        // Unknown roles returns none
        yield return new object[]
        {
            new List<string>() { "Idk" },
            new List<UserDTO>().AsReadOnly()
        };
    }
}
