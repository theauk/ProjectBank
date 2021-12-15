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
    [InlineData("Supervisor")]
    [InlineData("Student")]
    [InlineData("Admin")]
    [InlineData("all")]
    public async Task Get_Role_returns_all_Users_with_the_specified_role(string role)
    {
        // Arrange
        var expected = Array.Empty<UserDTO>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.ReadAllByRoleAsync(role)).ReturnsAsync(expected);
        var controller = new UserController(repository.Object);

        // Act
        var actual = await controller.Get(role);

        // Assert
        Assert.Equal(expected, actual);
    }
    
}