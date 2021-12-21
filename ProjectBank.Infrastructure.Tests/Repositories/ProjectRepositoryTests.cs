namespace ProjectBank.Infrastructure.Tests.Repositories;

public class ProjectRepositoryTests : RepoTests
{
    private readonly ProjectRepository _repository;

    public ProjectRepositoryTests() => _repository = new ProjectRepository(_context);

    [Fact]
    public async Task CreateAsync_creates_new_project_with_generated_id()
    {
        var project = new ProjectCreateDTO
        {
            Name = "Bachelor Project",
            Description = "The final project of SWU.",
            ExistingTagIds = new HashSet<int> { 1 },
            NewTagDTOs = new HashSet<TagCreateDTO> { new TagCreateDTO { Value = "Winter 2020", TagGroupId = 1 } },
            UserIds = new HashSet<int> { 4, 5 }
        };

        var response = await _repository.CreateAsync(project, "paolo@itu.dk");
        var actualProject = _context.Projects.Find(5)?.ToDTO();

        Assert.Equal(Response.Created, response);
        Assert.Equal(new ProjectDTO
        {
            Id = 5,
            Name = "Bachelor Project",
            Description = "The final project of SWU.",
            Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 4, Name = "Paolo", Email = "paolo@itu.dk", Role = Role.Admin }, new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin } },
            Tags = new List<TagDTO> { new TagDTO { Id = 1, Value = "Spring 2022" }, new TagDTO { Id = 17, Value = "Winter 2020" } }
        }, actualProject);
    }

    [Fact]
    public async Task CreateAsync_given_invalid_Project_Owner_email_returns_BadRequest()
    {
        var project = new ProjectCreateDTO
        {
            Name = "Bachelor Project",
            Description = "The final project of SWU.",
            ExistingTagIds = new HashSet<int>() { 1 },
            UserIds = new HashSet<int>() { 4, 6 }
        };

        var actual = await _repository.CreateAsync(project, "test@itu.dk");

        Assert.Equal(Response.BadRequest, actual);
    }
    
    [Fact]
    public async Task CreateAsync_given_null_Tag__returns_BadRequest()
    {
        var project = new ProjectCreateDTO
        {
            Name = "Bachelor Project",
            Description = "The final project of SWU.",
            ExistingTagIds = new HashSet<int>() { 1,999 },
            UserIds = new HashSet<int>() { 4, 6 }
        };

        var actual = await _repository.CreateAsync(project, "paolo@itu.dk");

        Assert.Equal(Response.BadRequest, actual);
    }

    [Fact]
    public async Task CreateAsync_given_non_existing_User_Id_returns_BadRequest()
    {
        var project = new ProjectCreateDTO
        {
            Name = "Bachelor Project",
            Description = "The final project of SWU.",
            ExistingTagIds = new HashSet<int>() { 1 },
            UserIds = new HashSet<int>() { 4, 43 }
        };

        var actual = await _repository.CreateAsync(project, "paolo@itu.dk");

        Assert.Equal(Response.BadRequest, actual);
    }

    [Fact]
    public async Task CreateAsync_given_non_existing_University_returns_BadRequest()
    {
        var project = new ProjectCreateDTO
        {
            Name = "Bachelor Project",
            Description = "The final project of SWU.",
            ExistingTagIds = new HashSet<int>() { 1 },
            UserIds = new HashSet<int>() { 4, 1 }
        };

        var actual = await _repository.CreateAsync(project, "paolo@sdu.dk");
        Assert.Equal(Response.BadRequest, actual);
    }

    [Fact]
    public async Task DeleteAsync_given_existing_id_deletes()
    {
        var actual = await _repository.DeleteAsync(3);

        Assert.Equal(Response.Deleted, actual);
        Assert.Null(await _context.Projects.FindAsync(3));
    }

    [Fact]
    public async Task DeleteAsync_given_non_existing_id_returns_NotFound()
    {
        var actual = await _repository.DeleteAsync(0);
        Assert.Equal(Response.NotFound, actual);
    }

    [Fact]
    public async Task ReadAsync_given_id_exists_returns_Project()
    {
        var actual = (await _repository.ReadAsync(3)).Value;

        Assert.NotNull(actual);
        Assert.Equal(new ProjectDTO
        {
            Id = 3,
            Name = "Make an app!",
            Description = "Like a dating app, or something. Just something we can sell for a lot of money.",
            Tags = new List<TagDTO> { new TagDTO { Id = 2, Value = "Autumn 2022" }, new TagDTO { Id = 3, Value = "Spring 2023" }, new TagDTO { Id = 13, Value = "JavaScript (yuckier)" }, new TagDTO { Id = 14, Value = "Bachelor" } }.OrderBy(t => t.Value).ToList(),
            Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 1, Name = "Marco", Email = "marco@itu.dk", Role = Role.Admin }, new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk", Role = Role.Admin } }
        }, actual);
    }

    [Fact]
    public async Task ReadAsync_given_non_existing_id_returns_Null()
    {
        var actual = (await _repository.ReadAsync(33)).Value;
        Assert.Null(actual);
    }

    [Fact]
    public async Task ReadAllAsync_returns_all_Projects_across_all_Universities()
    {
        var projects = await _repository.ReadAllAsync();
        Assert.Collection(projects,
            project => Assert.Equal(new ProjectDTO { Id = 1, Name = "Invent AI", Description = "Your task, should you choose to accept it, is to threaten the existence of life on earth.", Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin }, new UserDTO { Id = 3, Name = "Bjørn", Email = "bjorn@itu.dk", Role = Role.Admin } }, Tags = new List<TagDTO>() { new TagDTO { Id = 1, Value = "Spring 2022" }, new TagDTO { Id = 10, Value = "C" }, new TagDTO { Id = 11, Value = "C++" }, new TagDTO { Id = 16, Value = "PhD" } }.OrderBy(t => t.Value).ToList() }, project),
            project => Assert.Equal(new ProjectDTO { Id = 2, Name = "Project Bank", Description = "Replace our current system.", Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin }, new UserDTO { Id = 4, Name = "Paolo", Email = "paolo@itu.dk", Role = Role.Admin } }, Tags = new List<TagDTO> { new TagDTO { Id = 1, Value = "Spring 2022" }, new TagDTO { Id = 7, Value = ".NET" }, new TagDTO { Id = 14, Value = "Bachelor" } }.OrderBy(t => t.Value).ToList() }, project),
            project => Assert.Equal(new ProjectDTO { Id = 3, Name = "Make an app!", Description = "Like a dating app, or something. Just something we can sell for a lot of money.", Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk", Role = Role.Admin }, new UserDTO { Id = 1, Name = "Marco", Email = "marco@itu.dk", Role = Role.Admin } }, Tags = new List<TagDTO> { new TagDTO { Id = 14, Value = "Bachelor" }, new TagDTO { Id = 2, Value = "Autumn 2022" }, new TagDTO { Id = 3, Value = "Spring 2023" }, new TagDTO { Id = 13, Value = "JavaScript (yuckier)" } }.OrderBy(t => t.Value).ToList() }, project),
            project => Assert.Equal(new ProjectDTO { Id = 4, Name = "Idk", Description = "Out of ideas for projects.", Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 8, Name = "Heidi", Email = "heidi@ruc.dk", Role = Role.Supervisor } }, Tags = new List<TagDTO>() }, project)
        );
    }

    [Theory]
    [MemberData(nameof(GetProjects))]
    public async Task ReadAllByUniversityAsync_returns_all_Projects_from_University(string email, IReadOnlyCollection<ProjectDTO> expected)
    {
        var actual = await _repository.ReadAllByUniversityAsync(email);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetFilteredProjects))]
    public async Task ReadFilteredAsync_returns_Projects_with_the_desired_Supervisors_and_Tags(string email, IList<int> tagIds, IList<int> supervisorIds, IReadOnlyCollection<ProjectDTO> expected)
    {
        var actual = await _repository.ReadFilteredAsync(email, tagIds, supervisorIds);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task UpdateAsync_updates_existing_Project()
    {
        var project = new ProjectUpdateDTO
        {
            Name = "Extreme Math Project",
            Description = "Prove even harder stuff.",
            ExistingTagIds = new HashSet<int>(),
            UserIds = new HashSet<int>() { 1, 2 }
        };

        var response = await _repository.UpdateAsync(1, project);

        Assert.Equal(Response.Updated, response);

        var mathProject = (await _repository.ReadAsync(1)).Value;

        Assert.NotNull(mathProject);
        Assert.Empty(mathProject?.Tags);
    }

    [Fact]
    public async Task UpdateAsync_given_non_existing_id_returns_NotFound()
    {
        var project = new ProjectUpdateDTO
        {
            Name = "Extreme Math Project",
            Description = "Prove even harder stuff.",
            ExistingTagIds = new HashSet<int>() { 1 },
            UserIds = new HashSet<int>() { 1, 2 }
        };

        var actual = await _repository.UpdateAsync(33, project);

        Assert.Equal(Response.NotFound, actual);
    }

    [Fact]
    public async Task UpdateAsync_given_non_existing_user_id_returns_BadRequest()
    {
        var project = new ProjectUpdateDTO
        {
            Name = "Extreme Math Project",
            Description = "Prove even harder stuff.",
            ExistingTagIds = new HashSet<int>() { 1 },
            UserIds = new HashSet<int>() { 1, 9 }
        };

        var actual = await _repository.UpdateAsync(1, project);

        Assert.Equal(Response.BadRequest, actual);
    }

    [Fact]
    public async Task UpdateAsync_given_non_existing_Tag_id_returns_BadRequest()
    {
        var project = new ProjectUpdateDTO
        {
            Name = "Extreme Math Project",
            Description = "Prove even harder stuff.",
            ExistingTagIds = new HashSet<int>() { 33 },
            UserIds = new HashSet<int>() { 1, 9 }
        };

        var actual = await _repository.UpdateAsync(1, project);
        Assert.Equal(Response.BadRequest, actual);
    }

    [Fact]
    public async Task UpdateAsync_given_non_existing_TagGroup_id_returns_BadRequest()
    {
        var project = new ProjectUpdateDTO
        {
            Name = "Extreme Math Project",
            Description = "Prove even harder stuff.",
            ExistingTagIds = new HashSet<int>() { 1 },
            NewTagDTOs = new HashSet<TagCreateDTO> { new TagCreateDTO { Value = "?", TagGroupId = 33 } },
            UserIds = new HashSet<int>() { 1, 9 }
        };

        var actual = await _repository.UpdateAsync(1, project);
        Assert.Equal(Response.BadRequest, actual);
    }

    public static IEnumerable<object[]> GetProjects()
    {
        yield return new object[]
        {
            "test@itu.dk",
            new List<ProjectDTO>()
            {
                new ProjectDTO { Id = 1, Name = "Invent AI", Description = "Your task, should you choose to accept it, is to threaten the existence of life on earth.", Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin }, new UserDTO { Id = 3, Name = "Bjørn", Email = "bjorn@itu.dk", Role = Role.Admin } }, Tags = new List<TagDTO>() { new TagDTO { Id = 1, Value = "Spring 2022" }, new TagDTO { Id = 10, Value = "C" }, new TagDTO { Id = 11, Value = "C++"}, new TagDTO { Id = 16, Value = "PhD" } }.OrderBy(t => t.Value).ToList() },
                new ProjectDTO { Id = 2, Name = "Project Bank", Description = "Replace our current system.", Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin }, new UserDTO { Id = 4, Name = "Paolo", Email = "paolo@itu.dk", Role = Role.Admin } }, Tags = new List<TagDTO> { new TagDTO { Id = 1, Value = "Spring 2022" }, new TagDTO { Id = 7, Value = ".NET" }, new TagDTO { Id = 14, Value = "Bachelor" }  }.OrderBy(t => t.Value).ToList() },
                new ProjectDTO { Id = 3, Name = "Make an app!", Description =  "Like a dating app, or something. Just something we can sell for a lot of money.", Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk", Role = Role.Admin }, new UserDTO { Id = 1, Name = "Marco", Email = "marco@itu.dk", Role = Role.Admin } }, Tags = new List<TagDTO> { new TagDTO { Id = 14, Value = "Bachelor" }, new TagDTO { Id = 2, Value = "Autumn 2022" }, new TagDTO { Id = 3, Value = "Spring 2023" }, new TagDTO { Id = 13, Value = "JavaScript (yuckier)" } }.OrderBy(t => t.Value).ToList() },
            }.AsReadOnly()
        };

        yield return new object[]
        {
            "test@ruc.dk",
            new List<ProjectDTO>()
            {
                new ProjectDTO { Id = 4, Name = "Idk", Description = "Out of ideas for projects.", Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 8, Name = "Heidi", Email = "heidi@ruc.dk", Role = Role.Supervisor } }, Tags = new List<TagDTO>() { }.OrderBy(t => t.Value).ToList() }
            }.AsReadOnly()
        };
    }

    public static IEnumerable<object[]> GetFilteredProjects()
    {
        // Only 1 supervisors
        yield return new object[]
        {
            "test@itu.dk",
            new List<int> { },
            new List<int> { 1 },
            new List<ProjectDTO>
            {
                new ProjectDTO { Id = 3, Name = "Make an app!", Description =  "Like a dating app, or something. Just something we can sell for a lot of money.", Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk", Role = Role.Admin }, new UserDTO { Id = 1, Name = "Marco", Email = "marco@itu.dk", Role = Role.Admin } }, Tags = new List<TagDTO> { new TagDTO { Id = 14, Value = "Bachelor" }, new TagDTO { Id = 2, Value = "Autumn 2022" }, new TagDTO { Id = 3, Value = "Spring 2023" }, new TagDTO { Id = 13, Value = "JavaScript (yuckier)" } }.OrderBy(t => t.Value).ToList() },
            }.AsReadOnly()
        };

        // Many supervisors TRUE
        yield return new object[]
        {
            "test@itu.dk",
            new List<int> { },
            new List<int> { 1, 2 },
            new List<ProjectDTO>
            {
                new ProjectDTO { Id = 3, Name = "Make an app!", Description =  "Like a dating app, or something. Just something we can sell for a lot of money.", Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk", Role = Role.Admin }, new UserDTO { Id = 1, Name = "Marco", Email = "marco@itu.dk", Role = Role.Admin } }, Tags = new List<TagDTO> { new TagDTO { Id = 14, Value = "Bachelor" }, new TagDTO { Id = 2, Value = "Autumn 2022" }, new TagDTO { Id = 3, Value = "Spring 2023" }, new TagDTO { Id = 13, Value = "JavaScript (yuckier)" } }.OrderBy(t => t.Value).ToList() },
            }.AsReadOnly()
        };

        // Many supervisors FALSE
        yield return new object[]
        {
            "test@itu.dk",
            new List<int> { },
            new List<int> { 1, 3 },
            new List<ProjectDTO>().AsReadOnly()
        };

        // Only 1 tag
        yield return new object[]
        {
            "test@itu.dk",
            new List<int> { 1 },
            new List<int> { },
            new List<ProjectDTO>
            {
                new ProjectDTO { Id = 1, Name = "Invent AI", Description = "Your task, should you choose to accept it, is to threaten the existence of life on earth.", Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin }, new UserDTO { Id = 3, Name = "Bjørn", Email = "bjorn@itu.dk", Role = Role.Admin } }, Tags = new List<TagDTO>() { new TagDTO { Id = 1, Value = "Spring 2022" }, new TagDTO { Id = 10, Value = "C" }, new TagDTO { Id = 11, Value = "C++"}, new TagDTO { Id = 16, Value = "PhD" } }.OrderBy(t => t.Value).ToList() },
                new ProjectDTO { Id = 2, Name = "Project Bank", Description = "Replace our current system.", Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin }, new UserDTO { Id = 4, Name = "Paolo", Email = "paolo@itu.dk", Role = Role.Admin } }, Tags = new List<TagDTO> { new TagDTO { Id = 1, Value = "Spring 2022" }, new TagDTO { Id = 7, Value = ".NET" }, new TagDTO { Id = 14, Value = "Bachelor" }  }.OrderBy(t => t.Value).ToList() },
            }.AsReadOnly()
        };

        // Many tags FALSE
        yield return new object[]
        {
            "test@itu.dk",
            new List<int> { 1, 13 },
            new List<int> { },
            new List<ProjectDTO>().AsReadOnly()
        };

        // Many tags TRUE
        yield return new object[]
        {
            "test@itu.dk",
            new List<int> { 2, 3 },
            new List<int> { },
            new List<ProjectDTO>
            {
                new ProjectDTO { Id = 3, Name = "Make an app!", Description =  "Like a dating app, or something. Just something we can sell for a lot of money.", Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk", Role = Role.Admin }, new UserDTO { Id = 1, Name = "Marco", Email = "marco@itu.dk", Role = Role.Admin } }, Tags = new List<TagDTO> { new TagDTO { Id = 14, Value = "Bachelor" }, new TagDTO { Id = 2, Value = "Autumn 2022" }, new TagDTO { Id = 3, Value = "Spring 2023" }, new TagDTO { Id = 13, Value = "JavaScript (yuckier)" } }.OrderBy(t => t.Value).ToList() },
            }.AsReadOnly()
        };

        // One each TRUE
        yield return new object[]
        {
            "test@itu.dk",
            new List<int> { 1 },
            new List<int> { 5 },
            new List<ProjectDTO>
            {
                new ProjectDTO { Id = 1, Name = "Invent AI", Description = "Your task, should you choose to accept it, is to threaten the existence of life on earth.", Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin }, new UserDTO { Id = 3, Name = "Bjørn", Email = "bjorn@itu.dk", Role = Role.Admin } }, Tags = new List<TagDTO>() { new TagDTO { Id = 1, Value = "Spring 2022" }, new TagDTO { Id = 10, Value = "C" }, new TagDTO { Id = 11, Value = "C++"}, new TagDTO { Id = 16, Value = "PhD" } }.OrderBy(t => t.Value).ToList() },
                new ProjectDTO { Id = 2, Name = "Project Bank", Description = "Replace our current system.", Supervisors = new HashSet<UserDTO> { new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Admin }, new UserDTO { Id = 4, Name = "Paolo", Email = "paolo@itu.dk", Role = Role.Admin } }, Tags = new List<TagDTO> { new TagDTO { Id = 1, Value = "Spring 2022" }, new TagDTO { Id = 7, Value = ".NET" }, new TagDTO { Id = 14, Value = "Bachelor" }  }.OrderBy(t => t.Value).ToList() },
            }.AsReadOnly()
        };

        // One each FALSE
        yield return new object[]
        {
            "test@itu.dk",
            new List<int> { 6 },
            new List<int> { 6 },
            new List<ProjectDTO>().AsReadOnly()
        };

        // One TRUE one FALSE
        yield return new object[]
        {
            "test@itu.dk",
            new List<int> { 1 },
            new List<int> { 6 },
            new List<ProjectDTO>().AsReadOnly()
        };

        // One FALSE one TRUE
        yield return new object[]
        {
            "test@itu.dk",
            new List<int> { 6 },
            new List<int> { 5 },
            new List<ProjectDTO>().AsReadOnly()
        };

        // Non existing university
        yield return new object[]
        {
            "test@sdu.dk",
            new List<int> { 6 },
            new List<int> { 5 },
            new List<ProjectDTO>().AsReadOnly()
        };

        // Negative values
        yield return new object[]
        {
            "test@itu.dk",
            new List<int> { -6 },
            new List<int> { -5 },
            new List<ProjectDTO>().AsReadOnly()
        };
    }
}
