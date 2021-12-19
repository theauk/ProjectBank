namespace ProjectBank.Infrastructure.Tests.Repositories;

public class TagGroupRepositoryTest : RepoTests
{
    private readonly TagGroupRepository _repository;

    public TagGroupRepositoryTest() => _repository = new TagGroupRepository(_context);

    [Fact]
    public async Task CreateAsync_given_existing_University_creates_new_TagGroup_with_generated_id_and_returns_Created()
    {
        //Arrange
        var taggroup = new TagGroupCreateDTO()
        {
            Name = "Programme",
            NewTagsDTOs = new HashSet<TagCreateDTO>()
            {
                new() {Value = "CompSci"},
                new() {Value = "Economics"},
                new() {Value = "Medicine"}
            },
            SupervisorCanAddTag = false,
            RequiredInProject = false,
            TagLimit = 999,
        };

        //Act
        var response = await _repository.CreateAsync(taggroup, "test@itu.dk");
        var actual = _context.TagGroups.First(tg => tg.Name == taggroup.Name).ToDTO();

        //Assert 
        Assert.Equal(Response.Created, response);
        Assert.Equal(taggroup.Name, actual.Name);
        Assert.Equal(taggroup.RequiredInProject, actual.RequiredInProject);
        Assert.Equal(taggroup.SupervisorCanAddTag, actual.SupervisorCanAddTag);
        Assert.Equal(taggroup.TagLimit, actual.TagLimit);
        Assert.Equal(taggroup.NewTagsDTOs.Count, actual.TagDTOs.Count);
        Assert.Equal(new HashSet<TagDTO> { new TagDTO { Id = 17, Value = "CompSci" }, new TagDTO { Id = 18, Value = "Economics" }, new TagDTO { Id = 19, Value = "Medicine" } }, actual.TagDTOs);
    }

    [Fact]
    public async Task CreateAsync_given_non_existing_University_returns_BadRequest()
    {
        //Arrange
        var taggroup = new TagGroupCreateDTO()
        {
            Name = "Programme",
            NewTagsDTOs = new HashSet<TagCreateDTO>()
            {
                new() {Value = "CompSci"},
                new() {Value = "Economics"},
                new() {Value = "Medicine"}
            },
            SupervisorCanAddTag = false,
            RequiredInProject = false,
            TagLimit = 999,
        };

        //Act
        var response = await _repository.CreateAsync(taggroup, "test@dtu.dk");
        Assert.Equal(Response.BadRequest, response);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task DeleteAsync_deletes_correctly(int id)
    {
        //Act
        var response = await _repository.DeleteAsync(id);

        //Assert
        Assert.Equal(Response.Deleted, response);
        Assert.Null(await _context.TagGroups.FindAsync(id));
        Assert.True((await _repository.ReadAsync(id)).IsNone);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(56)]
    [InlineData(100)]
    [InlineData(199)]
    [InlineData(int.MaxValue)]
    public async Task DeleteAsync_delete_nonexistent_returns_NotFound(int id) =>
        Assert.Equal(Response.NotFound, await _repository.DeleteAsync(id));

    [Fact]
    public async Task ReadAsync_given_existing_id_returns_TagGroup()
    {
        // Act
        var actual = (await _repository.ReadAsync(3)).Value;

        //Assert
        Assert.Equal(3, actual?.Id);
        Assert.Equal("Level", actual?.Name);
        Assert.Equal(3, actual?.TagDTOs.Count);
        Assert.False(actual?.SupervisorCanAddTag);
        Assert.True(actual?.RequiredInProject);
        Assert.Null(actual?.TagLimit);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(56)]
    [InlineData(100)]
    [InlineData(199)]
    [InlineData(int.MaxValue)]
    public async Task ReadAsync_given_non_existaing_id_returns_None(int id)
    {
        Assert.True((await _repository.ReadAsync(id)).IsNone);
    }

    [Fact]
    public async Task ReadAllAsync_returns_all_TagGroups_across_all_Universities()
    {
        // Arrange
        var semester = new List<TagDTO>
        {
            new TagDTO { Id = 1, Value = "Spring 2022" },
            new TagDTO { Id = 2, Value = "Autumn 2022" },
            new TagDTO { Id = 3, Value = "Spring 2023" },
            new TagDTO { Id = 4, Value = "Autumn 2023" },
            new TagDTO { Id = 5, Value = "Spring 2024" },
            new TagDTO { Id = 6, Value = "Autumn 2024" }
        };

        var proLang = new List<TagDTO>
        {
            new TagDTO { Id = 7, Value = ".NET" },
            new TagDTO { Id = 8, Value = "Python" },
            new TagDTO { Id = 9, Value = "Rust" },
            new TagDTO { Id = 10, Value = "C" },
            new TagDTO { Id = 11, Value = "C++" },
            new TagDTO { Id = 12, Value = "Java (yuck)" },
            new TagDTO { Id = 13, Value = "JavaScript (yuckier)" }
        };

        var level = new List<TagDTO>
        {
            new TagDTO { Id = 14, Value = "Bachelor" },
            new TagDTO { Id = 15, Value = "Master" },
            new TagDTO { Id = 16, Value = "PhD" }
        };

        // Act
        var actual = await _repository.ReadAllAsync();
        Assert.Collection(actual,
            tagGroup => Assert.Equal(new TagGroupDTO { Id = 1, Name = "Semester", RequiredInProject = true, SupervisorCanAddTag = false, TagLimit = 2, TagDTOs = semester }, tagGroup),
            tagGroup => Assert.Equal(new TagGroupDTO { Id = 2, Name = "Programming Language", RequiredInProject = false, SupervisorCanAddTag = true, TagLimit = 3, TagDTOs = proLang }, tagGroup),
            tagGroup => Assert.Equal(new TagGroupDTO { Id = 3, Name = "Level", RequiredInProject = true, SupervisorCanAddTag = false, TagLimit = null, TagDTOs = level }, tagGroup),
            tagGroup => Assert.Equal(new TagGroupDTO { Id = 4, Name = "Language", RequiredInProject = false, SupervisorCanAddTag = false, TagLimit = null, TagDTOs = new List<TagDTO>() }, tagGroup)
        );
    }

    [Theory]
    [MemberData(nameof(GetTagGroups))]

    public async Task ReadAllByUniversityAsync_returns_all_TagGroups_from_University(string email, IReadOnlyCollection<TagGroupDTO> expected)
    {
        var actual = await _repository.ReadAllByUniversityAsync(email);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task UpdatesAsync_given_existing_id_Updates_TagGroup_and_returns_Updated()
    {
        //Arrange
        var tags = new HashSet<string>() 
        { 
            "Autumn 2022", 
            "Spring 2023", 
            "Autumn 2023", 
            "Spring 23" 
        };
        var tagGroupUpdate = new TagGroupUpdateDTO()
        { 
            Name = "Semester (Updated)", 
            SupervisorCanAddTag = false, 
            RequiredInProject = true, 
            SelectedTagValues = tags 
        };

        // Act
        var update = await _repository.UpdateAsync(1, tagGroupUpdate);
        var readUpdatedTagGroup = _context.TagGroups.Find(1)?.ToDTO();
        var updatedTags = readUpdatedTagGroup?.TagDTOs;

        //Assert
        Assert.Equal(Response.Updated, update);
        Assert.False(readUpdatedTagGroup?.SupervisorCanAddTag);
        Assert.True(readUpdatedTagGroup?.RequiredInProject);
        Assert.Equal("Semester (Updated)", readUpdatedTagGroup?.Name);
        Assert.Contains(updatedTags, t => t.Id != 12 && t.Value != "Spring 2022");
        Assert.Contains(updatedTags, t => t.Value == "Spring 2023");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(56)]
    [InlineData(100)]
    [InlineData(199)]
    [InlineData(int.MaxValue)]
    public async Task UpdateAsync_given_non_existing_id_returns_NotFound(int id)
    {
        //Arrange
        var tags = new HashSet<string>() 
        { 
            "spring 23" 
        };
        var tagGroupUpdate = new TagGroupUpdateDTO()
        { 
            Name = "Semester (Updated)", 
            SupervisorCanAddTag = false, 
            RequiredInProject = true, 
            SelectedTagValues = tags 
        };

        // Act
        var response = await _repository.UpdateAsync(id, tagGroupUpdate);

        //Assert
        Assert.Equal(Response.NotFound, response);
    }

    public static IEnumerable<object[]> GetTagGroups()
    {
        var semester = new List<TagDTO>
        {
            new TagDTO { Id = 1, Value = "Spring 2022" },
            new TagDTO { Id = 2, Value = "Autumn 2022" },
            new TagDTO { Id = 3, Value = "Spring 2023" },
            new TagDTO { Id = 4, Value = "Autumn 2023" },
            new TagDTO { Id = 5, Value = "Spring 2024" },
            new TagDTO { Id = 6, Value = "Autumn 2024" }
        };

        var proLang = new List<TagDTO>
        {
            new TagDTO { Id = 7, Value = ".NET" },
            new TagDTO { Id = 8, Value = "Python" },
            new TagDTO { Id = 9, Value = "Rust" },
            new TagDTO { Id = 10, Value = "C" },
            new TagDTO { Id = 11, Value = "C++" },
            new TagDTO { Id = 12, Value = "Java (yuck)" },
            new TagDTO { Id = 13, Value = "JavaScript (yuckier)" }
        };

        var level = new List<TagDTO>
        {
            new TagDTO { Id = 14, Value = "Bachelor" },
            new TagDTO { Id = 15, Value = "Master" },
            new TagDTO { Id = 16, Value = "PhD" }
        };

        // From itu
        yield return new object[]
        {
            "test@itu.dk",
            new List<TagGroupDTO>
            {
                new TagGroupDTO { Id = 1, Name = "Semester", RequiredInProject = true, SupervisorCanAddTag = false, TagLimit = 2, TagDTOs = semester },
                new TagGroupDTO { Id = 2, Name = "Programming Language", RequiredInProject = false, SupervisorCanAddTag = true, TagLimit = 3, TagDTOs = proLang },
                new TagGroupDTO { Id = 3, Name = "Level", RequiredInProject = true, SupervisorCanAddTag = false, TagLimit = null, TagDTOs = level },
            }
        };

        // From ruc
         yield return new object[]
        {
            "test@ruc.dk",
            new List<TagGroupDTO>
            {
                new TagGroupDTO { Id = 4, Name = "Language", RequiredInProject = false, SupervisorCanAddTag = false, TagLimit = null, TagDTOs = new List<TagDTO>() },
            }
        };
    }
}
