namespace ProjectBank.Server.Tests.Controllers;

public class UniversityControllerTests
{
    [Fact]
    public async Task Post_given_not_already_existing_domain_creates_University()
    {
        // Arrange
        var toCreate = new UniversityCreateDTO
        {
            DomainName = "itu.dk",
        };
        var response = Response.Created;
        var created = new UniversityDTO
        {
            DomainName = "itu.dk",
            Projects = new HashSet<ProjectDTO>(),
            TagGroups = new HashSet<TagGroupDTO>(),
            Users = new HashSet<UserDTO>()
        };
        var repository = new Mock<IUniversityRepository>();
        repository.Setup(m => m.CreateAsync(toCreate)).ReturnsAsync(response);

        var controller = new UniversityController(repository.Object);

        // Act
        var result = await controller.Post(toCreate) as CreatedResult;

        // Assert
        Assert.Equal(Response.Created, result?.Value);
        Assert.Equal("Get", result?.Location);
    }

    [Fact]
    public async Task Post_given_already_existing_domain_does_not_create_University()
    {
        // Arrange
        var toCreate = new UniversityCreateDTO
        {
            DomainName = "itu.dk",
        };
        var response = Response.Conflict;
        var repository = new Mock<IUniversityRepository>();
        repository.Setup(m => m.CreateAsync(toCreate)).ReturnsAsync(response);

        var controller = new UniversityController(repository.Object);

        // Act
        var result = await controller.Post(toCreate) as ConflictResult;

        // Assert
        Assert.Equal(409, result?.StatusCode);
    }

    [Fact]
    public async Task Post_given_already_existing_domain_does_not_create_University()
    {
        // Arrange
        var toCreate = new UniversityCreateDTO
        {
            DomainName = "itu.dk",
        };
        var response = Response.Conflict;
        var repository = new Mock<IUniversityRepository>();
        repository.Setup(m => m.CreateAsync(toCreate)).ReturnsAsync(response);

        var controller = new UniversityController(repository.Object);

        // Act
        var result = await controller.Post(toCreate) as CreatedAtActionResult;

        // Assert
        Assert.Equal(Response.Conflict, result?.Value);
    }

    [Fact]
    public async Task Get_given_existing_domain_returns_University()
    {
        // Arrange
        var repository = new Mock<IUniversityRepository>();
        var tagGroup = new UniversityDTO
        {
            DomainName = "itu.dk",
            Projects = new HashSet<ProjectDTO>(),
            TagGroups = new HashSet<TagGroupDTO>(),
            Users = new HashSet<UserDTO>()
        };
        repository.Setup(m => m.ReadAsync("itu.dk")).ReturnsAsync(tagGroup);
        var controller = new UniversityController(repository.Object);

        // Act
        var response = await controller.Get("itu.dk");

        // Assert
        Assert.Equal(tagGroup, response.Value);
    }

    [Fact]
    public async Task Get_given_non_existing_domain_returns_NotFound()
    {
        // Arrange
        var repository = new Mock<IUniversityRepository>();
        repository.Setup(m => m.ReadAsync("dtu.dk")).ReturnsAsync(default(UniversityDTO));
        var controller = new UniversityController(repository.Object);

        // Act
        var response = await controller.Get("dtu.dk");

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async Task Get_returns_all_Universities()
    {
        // Arrange
        var repository = new Mock<IUniversityRepository>();
        var expected = Array.Empty<UniversityDTO>();
        repository.Setup(m => m.ReadAllAsync()).ReturnsAsync(expected);
        var controller = new UniversityController(repository.Object);

        // Act
        var actual = await controller.GetAll();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Delete_given_existing_domain_returns_NoContent()
    {
        // Arrange
        var repository = new Mock<ITagGroupRepository>();
        repository.Setup(m => m.DeleteAsync(1)).ReturnsAsync(Response.Deleted);
        var controller = new TagGroupController(repository.Object);

        // Act
        var response = await controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task Delete_given_non_existing_id_returns_NotFound()
    {
        // Arrange
        var repository = new Mock<IUniversityRepository>();
        repository.Setup(m => m.DeleteAsync("dtu.dk")).ReturnsAsync(Response.NotFound);
        var controller = new UniversityController(repository.Object);

        // Act
        var response = await controller.Delete("dtu.dk");

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }
}
