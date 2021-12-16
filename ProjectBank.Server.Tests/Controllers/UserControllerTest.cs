namespace ProjectBank.Server.Tests.Controllers;

public class UserControllerTests
{
    [Fact]
    public async Task Get_Filter_returns_all_Users_with_at_least_one_Project()
    {
        // Arrange
        var expected = Array.Empty<UserDTO>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.ReadAllActiveAsync()).ReturnsAsync(expected);
        var controller = new UserController(repository.Object);

        // Act
        var actual = await controller.Get();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetRoles))]
    public async Task Get_Role_returns_all_Users_with_the_specified_role(ISet<string> roles)
    {
        // Arrange
        var expected = Array.Empty<UserDTO>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.ReadAllByRoleAsync(roles)).ReturnsAsync(expected);
        var controller = new UserController(repository.Object);

        // Act
        var actual = await controller.Get(roles);

        // Assert
        Assert.Equal(expected, actual);
    }
    

    public static IEnumerable<object[]> GetRoles()
    {
        yield return new object[]
        {
            new HashSet<String>() { "Supervisor" },
        };

        yield return new object[]
        {
            new HashSet<string>() { "Admin" },
        };

        yield return new object[]
        {
            new HashSet<string>() { "Supervisor" },
        };

        yield return new object[]
        {
            new HashSet<string>() { "Student" },
        };

        yield return new object[]
        {
            new HashSet<string>() { "All" },
        };

        yield return new object[]
        {
            new HashSet<string>() { "Idk" },
        };
    }
}