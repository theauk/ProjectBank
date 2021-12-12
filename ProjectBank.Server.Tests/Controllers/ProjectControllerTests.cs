namespace ProjectBank.Server.Tests.Controllers;

public class ProjectControllerTests
{
    [Fact]
    public async Task Post_creates_Project()
    {
        // Arrange
        var toCreate = new ProjectCreateDTO
        { 
            Name = "Math Project"
        };
        var response = Response.Created;
        var created = new ProjectDTO 
        { 
            Id = 1,
            Name = "Math Project",
            Description = "Prove a lot of stuff.",
            Tags = new HashSet<TagDTO>() { new TagDTO { Id = 1, Value = "Math Theory" } },
            Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 1, Name = "Birgit" } }
        };
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.CreateAsync(toCreate, "test", "test@itu.dk")).ReturnsAsync(response);
        
        var controller = new ProjectController(repository.Object);

        // Act
        var result = await controller.Post(toCreate) as CreatedAtActionResult;

        // Assert
        Assert.Equal(Response.Created, result?.Value);
        Assert.Equal("Get", result?.ActionName);
    }

    [Fact]
    public async Task Get_returns_all_Projects_from_repository_given_no_Tags_or_Supervisors()
    {
        // Arange
        var expected = Array.Empty<ProjectDTO>();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.ReadAllAsync()).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);

        // Act
        var actual = await controller.Get(new List<int>(), new List<int>());

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Get_returns_all_Projects_with_Tag_MathTheory()
    {
        // Arange
        var expected = new List<ProjectDTO>()
        {
            new ProjectDTO
            {
                Id = 1,
                Name = "Math Project",
                Description = "Prove a lot of stuff.",
                Tags = new HashSet<TagDTO>() { new TagDTO { Id = 1, Value = "Math Theory" } },
                Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 1, Name = "Birgit" } }
            }
        };
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.ReadFilteredAsync(new List<int>() { 1 }, new List<int>())).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);

        // Act
        var actual = await controller.Get(new List<int>() { 1 }, new List<int>());

        // Assert
        Assert.Equal(expected, actual);   
    }

    [Fact]
    public async Task Get_returns_all_Projects_with_Supervisor_Birgit()
    {
        // Arange
        var expected = new List<ProjectDTO>()
        {
            new ProjectDTO
            {
                Id = 1,
                Name = "Math Project",
                Description = "Prove a lot of stuff.",
                Tags = new HashSet<TagDTO>() { new TagDTO { Id = 1, Value = "Math Theory" } },
                Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 1, Name = "Birgit" } }
            }
        };
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.ReadFilteredAsync(new List<int>() { }, new List<int>() { 1 } )).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);

        // Act
        var actual = await controller.Get(new List<int>(), new List<int>() { 1 });

        // Assert
        Assert.Equal(expected, actual);   
    }

    [Fact]
    public async Task Get_returns_all_Projects_with_Tag_MathTheory_and_Supervisor_Birgit()
    {
        // Arange
        var expected = new List<ProjectDTO>()
        {
            new ProjectDTO
            {
                Id = 1,
                Name = "Math Project",
                Description = "Prove a lot of stuff.",
                Tags = new HashSet<TagDTO>() { new TagDTO { Id = 1, Value = "Math Theory" } },
                Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 3, Name = "Birgit" } }
            }
        };
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.ReadFilteredAsync(new List<int>() { 1 }, new List<int>() { 3 } )).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);

        // Act
        var actual = await controller.Get(new List<int>() { 1 }, new List<int>() { 3 });

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
            Tags = new HashSet<TagDTO>() { new TagDTO { Id = 1, Value = "Math Theory" } },
            Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 3, Name = "Birgit" } }
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