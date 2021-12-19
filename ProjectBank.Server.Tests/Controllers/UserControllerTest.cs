using System.Linq;

namespace ProjectBank.Server.Tests.Controllers;

public class UserControllerTests
{
    private readonly ClaimsPrincipal _user;

    public UserControllerTests()
    {
        // Set up the claims principal since the controller needs the logged in user's name and email
        _user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("name", "First Last"),
            new Claim(ClaimTypes.Email, "test@itu.dk")
        }, "TestAuthentication"));
    }

    [Fact]
    public async Task Post_creates_User()
    {
        // Arrange
        var toCreate = new UserCreateDTO
        {
            Name = "Mads",
            Email = "test@itu.dk"
        };
        var response = Response.Created;
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.CreateAsync(toCreate)).ReturnsAsync(response);

        var controller = new UserController(repository.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext {User = _user}
        };

        // Act
        var result = await controller.Post(toCreate) as CreatedAtActionResult;

        // Assert
        Assert.Equal(Response.Created, result?.Value);
        Assert.Equal("Get", result?.ActionName);
    }

    [Fact]
    public async Task Post_given_non_existing_University_returns_BadRequest()
    {
        // Arrange
        var toCreate = new UserCreateDTO
        {
            Name = "Mads",
            Email = "test@itu.dk"
        };
        var response = Response.BadRequest;
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.CreateAsync(toCreate)).ReturnsAsync(response);

        var controller = new UserController(repository.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext {User = _user}
        };

        // Act
        var result = await controller.Post(toCreate) as CreatedAtActionResult;

        // Assert
        Assert.Equal(Response.BadRequest, result?.Value);
        Assert.Equal("Get", result?.ActionName);
    }

    [Fact]
    public async Task Get_Filter_returns_all_Users_with_at_least_one_Project()
    {
        // Arrange
        var expected = Array.Empty<UserDTO>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.ReadAllActiveAsync("test@itu.dk")).ReturnsAsync(expected);
        var controller = new UserController(repository.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext {User = _user}
        };

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
        repository.Setup(m => m.ReadAllByRoleAsync("test@itu.dk", roles)).ReturnsAsync(expected);
        var controller = new UserController(repository.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = _user }
        };

        // Act
        var actual = await controller.Get(roles);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Get_given_existing_id_returns_User()
    {
        // Arrange
        var expected = new UserDTO { Id = 1, Email = "test@itu.dk"};
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.ReadAsync(1)).ReturnsAsync(expected);
        var controller = new UserController(repository.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext {User = _user}
        };

        // Act
        var actual = await controller.Get(1);

        // Assert
        Assert.Equal(expected, actual.Value);
    }

    [Fact]
    public async Task Get_given_non_existing_id_returns_NotFound()
    {
        // Arrange
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.ReadAsync(1)).ReturnsAsync(default(UserDTO));
        var controller = new UserController(repository.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext {User = _user}
        };

        // Act
        var actual = await controller.Get(1);

        // Assert
        Assert.IsType<NotFoundResult>(actual.Result);
    }

    [Fact]
    public async Task Get_given_existing_email_returns_User()
    {
        // Arrange
        var expected = new UserDTO { Id = 1, Email = "test@itu.dk" };
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.ReadAsync("test@itu.dk")).ReturnsAsync(expected);
        var controller = new UserController(repository.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext {User = _user}
        };

        // Act
        var actual = await controller.Get("test@itu.dk");

        // Assert
        Assert.Equal(expected, actual.Value);
    }

    [Fact]
    public async Task Get_given_non_existing_email_returns_NotFound()
    {
        // Arrange
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.ReadAsync("test@itu.dk")).ReturnsAsync(default(UserDTO));
        var controller = new UserController(repository.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext {User = _user}
        };

        // Act
        var actual = await controller.Get("test@itu.dk");

        // Assert
        Assert.IsType<NotFoundResult>(actual.Result);
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