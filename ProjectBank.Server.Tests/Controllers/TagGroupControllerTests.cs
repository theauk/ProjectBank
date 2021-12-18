using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ProjectBank.Server.Tests.Controllers;

public class TagGroupControllerTests
{
    [Fact]
    public async Task Post_creates_TagGroup()
    {
        // Arrange
        var toCreate = new TagGroupCreateDTO
        {
            Name = "Semester",
            RequiredInProject = true,
            SupervisorCanAddTag = false,
            TagLimit = 2,
            NewTagsDTOs = new HashSet<TagCreateDTO>() { new TagCreateDTO { Value = "Fall", TagGroupId = 1 } }
        };
        var response = Response.Created;

        var repository = new Mock<ITagGroupRepository>();
        repository.Setup(m => m.CreateAsync(toCreate, "test@itu.dk")).ReturnsAsync(response);

        var controller = new TagGroupController(repository.Object);

        // Set up the claims principal since the controller needs the logged in user's name and email
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, "test@itu.dk")
        }, "TestAuthentication"));
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext {User = user}
        };

        // Act
        var result = await controller.Post(toCreate) as CreatedAtActionResult;

        // Assert
        Assert.Equal(Response.Created, result?.Value);
        Assert.Equal("Get", result?.ActionName);
    }

    [Fact]
    public async Task Get_returns_all_TagGroups_from_Repository()
    {
        // Arrange
        var expected = Array.Empty<TagGroupDTO>();
        var repository = new Mock<ITagGroupRepository>();
        repository.Setup(m => m.ReadAllAsync()).ReturnsAsync(expected);
        var controller = new TagGroupController(repository.Object);

        // Act
        var actual = await controller.Get();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Get_given_existing_id_returns_TagGroup()
    {
        // Arrange
        var repository = new Mock<ITagGroupRepository>();
        var tagGroup = new TagGroupDTO
        {
            Id = 1,
            Name = "Semester",
            RequiredInProject = true,
            SupervisorCanAddTag = false,
            TagLimit = 2,
            TagDTOs = new List<TagDTO>() { new TagDTO { Id = 1, Value = "Fall"} }
        };
        repository.Setup(m => m.ReadAsync(1)).ReturnsAsync(tagGroup);
        var controller = new TagGroupController(repository.Object);

        // Act
        var response = await controller.Get(1);

        // Assert
        Assert.True(tagGroup.Equals(response.Value));
    }

    [Fact]
    public async Task Get_given_non_existing_id_returns_NotFound()
    {
        // Arrange
        var repository = new Mock<ITagGroupRepository>();
        repository.Setup(m => m.ReadAsync(33)).ReturnsAsync(default(TagGroupDTO));
        var controller = new TagGroupController(repository.Object);

        // Act
        var response = await controller.Get(33);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async Task Put_given_existing_id_updates_TagGroup()
    {
        // Arrange
        var tagGroup = new TagGroupUpdateDTO();
        var repository = new Mock<ITagGroupRepository>();
        repository.Setup(m => m.UpdateAsync(1, tagGroup)).ReturnsAsync(Response.Updated);
        var controller = new TagGroupController(repository.Object);

        // Act
        var response = await controller.Put(1, tagGroup);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task Put_given_non_existing_id_returns_NotFound()
    {
        // Arrange
        var tagGroup = new TagGroupUpdateDTO();
        var repository = new Mock<ITagGroupRepository>();
        repository.Setup(m => m.UpdateAsync(33, tagGroup)).ReturnsAsync(Response.NotFound);
        var controller = new TagGroupController(repository.Object);

        // Act
        var response = await controller.Put(33, tagGroup);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task Delete_given_existing_id_returns_NoContent()
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
        var repository = new Mock<ITagGroupRepository>();
        repository.Setup(m => m.DeleteAsync(33)).ReturnsAsync(Response.NotFound);
        var controller = new TagGroupController(repository.Object);

        // Act
        var response = await controller.Delete(33);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }
}
