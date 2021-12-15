namespace ProjectBank.Infrastructure.Tests;

public class TagGroupRepositoryTest : RepoTests
{
    private readonly TagGroupRepository _repository;

    public TagGroupRepositoryTest() => _repository = new TagGroupRepository(_context);

    [Fact]
    public async Task ReadAsync_finds_correct_TagGroup()
    {
        // Act
        var result = await _repository.ReadAsync(3);
        if (result.IsNone)
            throw new MissingMemberException("Could not find TagGroup with id 3");
        TagGroupDTO tg = result.Value;
        
        //Assert
        Assert.Equal(3, tg.Id);
        Assert.Equal("Level", tg.Name);
        Assert.Equal(3, tg.TagDTOs.Count); //Ser om de har samme længde
        Assert.False(tg.SupervisorCanAddTag);
        Assert.True(tg.RequiredInProject);
        Assert.Null(tg.TagLimit);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(56)]
    [InlineData(100)]
    [InlineData(199)]
    [InlineData(int.MaxValue)]
    public async Task ReadAsync_on_nonexistant_id_returns_null(int id) =>
        Assert.True((await _repository.ReadAsync(id)).IsNone);


    [Fact]
    public async Task CreateAsync_creates_new_TagGroup_with_generated_id()
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
        var response  = await _repository.CreateAsync(taggroup);
        var actual = (await _repository.ReadAllAsync()).First(tg => tg.Name == taggroup.Name);
        
        //Assert 
        Assert.Equal(Response.Created, response);
        Assert.Equal(taggroup.Name, actual.Name);
        Assert.Equal(taggroup.RequiredInProject, actual.RequiredInProject);
        Assert.Equal(taggroup.SupervisorCanAddTag, actual.SupervisorCanAddTag);
        Assert.Equal(taggroup.TagLimit, actual.TagLimit);
        Assert.Equal(taggroup.NewTagsDTOs.Count, actual.TagDTOs.Count);
        foreach (var tag in taggroup.NewTagsDTOs)
            Assert.Contains(tag.Value, actual.TagDTOs.Select(t => t.Value));
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
    public async Task Updates_correctly()
    {
        //Arrange
        var tags = new HashSet<string>() {"Autumn 2022", "Spring 2023", "Autumn 2023","Spring 23"};
        var tagGroupUpdate = new TagGroupUpdateDTO()
            {Name = "Semester (Updated)", SupervisorCanAddTag = false, RequiredInProject = true, SelectedTagValues = tags};
        
        // Act
        var update =  await _repository.UpdateAsync(1, tagGroupUpdate);
        var readUpdatedTagGroup = await _repository.ReadAsync(1);
        var updatedTags = readUpdatedTagGroup.Value.TagDTOs;
        
        //Assert
        Assert.Equal(Response.Updated, update);
        Assert.False(readUpdatedTagGroup.Value.SupervisorCanAddTag);
        Assert.True(readUpdatedTagGroup.Value.RequiredInProject);
        Assert.Equal("Semester (Updated)", readUpdatedTagGroup.Value.Name);
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
    public async Task Update_TagGroupID_56_returns_NotFound(int id)
    {
        //Arrange
        var tags = new HashSet<string>() {"spring 23"};
        var tagGroupUpdate = new TagGroupUpdateDTO()
            {Name = "Semester (Updated)", SupervisorCanAddTag = false, RequiredInProject = true, SelectedTagValues = tags};
     
        // Act
        var response =  await _repository.UpdateAsync
            (id, tagGroupUpdate);
        
        //Assert
        Assert.Equal(Response.NotFound,response);
    }
}
