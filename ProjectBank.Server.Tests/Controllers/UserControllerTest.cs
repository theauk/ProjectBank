using System.Linq;

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
    public async Task Get_Role_returns_all_Users_with_the_specified_role(IList<string> roles, IReadOnlyCollection<UserDTO> expected)
    {
        // Arrange
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.ReadAllByRoleAsync(roles.ToHashSet())).ReturnsAsync(expected);
        var controller = new UserController(repository.Object);

        // Act
        var actual = await controller.Get(roles);

        // Assert
        Assert.Null(actual);
    }
    
    public static IEnumerable<object[]> GetRoles()
    {
        yield return new object[]
        {
            new List<string>() { "Supervisor" },
            new List<UserDTO>
            {
                new UserDTO 
                {
                    Name = "Jens",
                    Email = "jens@itu.dk",
                    Role = Role.Supervisor
                }
            }.AsReadOnly()
        };

        yield return new object[]
        {
            new List<string>() { "Admin" },
            new List<UserDTO>
            {
                new UserDTO 
                {
                    Name = "Jens",
                    Email = "jens@itu.dk",
                    Role = Role.Admin
                }
            }.AsReadOnly()
        };

        yield return new object[]
        {
            new List<string>() { "Student" },
            new List<UserDTO>
            {
                new UserDTO 
                {
                    Name = "Jens",
                    Email = "jens@itu.dk",
                    Role = Role.Student
                }
            }.AsReadOnly()
        };

        yield return new object[]
        {
            new List<string>() { "All" },
            new List<UserDTO>
            {
                new UserDTO 
                {
                    Name = "Jens",
                    Email = "jens@itu.dk",
                    Role = Role.Student
                }
            }.AsReadOnly()
        };

        yield return new object[]
        {
            new List<string>() { "Idk" },
            new List<UserDTO>().AsReadOnly()
        };
    }
}