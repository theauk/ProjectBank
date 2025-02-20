namespace ProjectBank.Server.Tests.Controllers;

public class ProjectControllerTests
{
    private readonly ClaimsPrincipal _user;

    public ProjectControllerTests()
    {
        // Set up the claims principal since the controller needs the logged in user's name and email
        _user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("name", "First Last"),
            new Claim(ClaimTypes.Email, "test@itu.dk")
        }, "TestAuthentication"));
    }

    [Fact]
    public async Task Post_creates_Project()
    {
        // Arrange
        var toCreate = new ProjectCreateDTO
        {
            Name = "Math Project"
        };
        var response = Response.Created;
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.CreateAsync(toCreate, "test@itu.dk")).ReturnsAsync(response);

        var controller = new ProjectController(repository.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext {User = _user}
        };

        // Act
        var result = await controller.Post(toCreate) as CreatedResult;

        // Assert
        Assert.Equal(Response.Created, result?.Value);
        Assert.Equal("Get", result?.Location);
    }

    [Fact]
    public async Task Get_returns_all_Projects_from_repository_associated_to_University_given_no_Tags_or_Supervisors()
    {
        // Arrange
        var expected = Array.Empty<ProjectDTO>();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.ReadAllAsync()).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext {User = _user}
        };

        // Act
        var actual = await controller.Get(new List<int>(), new List<int>());

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Get_returns_all_Projects_with_Tag_MathTheory_associated_to_University()
    {
        // Arrange
        var expected = new List<ProjectDTO>()
        {
            new()
            {
                Id = 1,
                Name = "Math Project",
                Description = "Prove a lot of stuff.",
                Tags = new HashSet<TagDTO>() {new TagDTO {Id = 1, Value = "Math Theory"}},
                Supervisors = new HashSet<UserDTO>() {new UserDTO {Id = 1, Name = "Birgit", Email = "birgit@itu.dk"}}
            }
        };
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.ReadFilteredAsync("test@itu.dk" ,new List<int>() {1}, new List<int>())).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext {User = _user}
        };

        // Act
        var actual = await controller.Get(new List<int>() {1}, new List<int>());

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Get_returns_all_Projects_with_Supervisor_Birgit_associated_to_University()
    {
        // Arrange
        var expected = new List<ProjectDTO>()
        {
            new ProjectDTO
            {
                Id = 1,
                Name = "Math Project",
                Description = "Prove a lot of stuff.",
                Tags = new HashSet<TagDTO>() {new TagDTO {Id = 1, Value = "Math Theory"}},
                Supervisors = new HashSet<UserDTO>() {new UserDTO {Id = 1, Name = "Brigit", Email = "brigit@itu.dk"}}
            }
        }.AsReadOnly();

        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.ReadFilteredAsync("test@itu.dk", new List<int>(), new List<int> {1})).ReturnsAsync(expected);
        repository.Setup(m => m.ReadAllByUniversityAsync("test@itu.dk")).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext {User = _user}
        };

        // Act
        var actual = await controller.Get(new List<int>(), new List<int> {1});

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Get_returns_all_Projects_with_Tag_MathTheory_and_Supervisor_Birgitassociated_to_University_associated_to_University()
    {
        // Arrange
        var expected = new List<ProjectDTO>()
        {
            new()
            {
                Id = 1,
                Name = "Math Project",
                Description = "Prove a lot of stuff.",
                Tags = new HashSet<TagDTO>() {new TagDTO {Id = 1, Value = "Math Theory"}},
                Supervisors = new HashSet<UserDTO>() {new UserDTO {Id = 3, Name = "Birgit", Email = "birgit@itu.dk"}}
            }
        };
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.ReadFilteredAsync("test@itu.dk", new List<int>() {1}, new List<int>() {3})).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext {User = _user}
        };

        // Act
        var actual = await controller.Get(new List<int>() {1}, new List<int>() {3});

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Get_given_existing_id_returns_Project()
    {
        // Arrange
        var repository = new Mock<IProjectRepository>();
        var project = new ProjectDTO
        {
            Id = 1,
            Name = "Math Project",
            Description = "Prove a lot of stuff.",
            Tags = new HashSet<TagDTO>() {new TagDTO {Id = 1, Value = "Math Theory"}},
            Supervisors = new HashSet<UserDTO>() {new UserDTO {Id = 3, Name = "Birgit", Email = "birgit@itu.dk"}}
        };
        repository.Setup(m => m.ReadAsync(1)).ReturnsAsync(project);
        var controller = new ProjectController(repository.Object);

        // Act
        var response = await controller.Get(1);

        // Assert
        Assert.Equal(project, response.Value);
    }

    [Fact]
    public async Task Get_given_non_existing_id_returns_NotFound()
    {
        // Arrange
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.ReadAsync(33)).ReturnsAsync(default(ProjectDTO));
        var controller = new ProjectController(repository.Object);

        // Act
        var response = await controller.Get(33);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async Task GetAll_returns_all_Projects_accross_all_Universities()
    {
        // Arrange
        var repository = new Mock<IProjectRepository>();
        var expected = new List<ProjectDTO>().AsReadOnly();
        repository.Setup(m => m.ReadAllAsync()).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);

        // Act
        var actual = await controller.GetAll();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Put_given_existing_id_updates_Project()
    {
        // Arrange
        var project = new ProjectUpdateDTO();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.UpdateAsync(1, project)).ReturnsAsync(Response.Updated);
        var controller = new ProjectController(repository.Object);

        // Act
        var response = await controller.Put(1, project);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task Put_given_non_existing_id_returns_NotFound()
    {
        // Arrange
        var project = new ProjectUpdateDTO();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.UpdateAsync(33, project)).ReturnsAsync(Response.NotFound);
        var controller = new ProjectController(repository.Object);

        // Act
        var response = await controller.Put(33, project);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task Put_given_non_existing_Property_values_id_returns_BadRequest()
    {
        // Arrange
        var project = new ProjectUpdateDTO { Id = 1, ExistingTagIds = new HashSet<int>() { 0 } };
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.UpdateAsync(1, project)).ReturnsAsync(Response.BadRequest);
        var controller = new ProjectController(repository.Object);

        // Act
        var response = await controller.Put(1, project);

        // Assert
        Assert.IsType<BadRequestResult>(response);
    }

    [Fact]
    public async Task Delete_given_existing_id_returns_NoContent()
    {
        // Arrange
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.DeleteAsync(1)).ReturnsAsync(Response.Deleted);
        var controller = new ProjectController(repository.Object);

        // Act
        var response = await controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task Delete_given_non_existing_id_returns_NotFound()
    {
        // Arrange
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.DeleteAsync(33)).ReturnsAsync(Response.NotFound);
        var controller = new ProjectController(repository.Object);

        // Act
        var response = await controller.Delete(33);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }
}
