using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Blazorise.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using ProjectBank.Core;
using ProjectBank.Core.DTOs;
using Xunit.Abstractions;

namespace ProjectBank.Server.Integration.Tests;

public class ProjectTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    
    

    public ProjectTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    //Expected DTOs
    //SupervisorDTOs
    private UserDTO marco = new UserDTO { Id = 1, Name = "Marco", Email = "marco@itu.dk", Role = Role.Supervisor };
    private UserDTO birgit = new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk", Role = Role.Supervisor };
    private UserDTO bjorn = new UserDTO { Id = 3, Name = "Bjørn", Email = "bjorn@itu.dk", Role = Role.Supervisor };
    private UserDTO paolo = new UserDTO { Id = 4, Name = "Paolo", Email = "paolo@itu.dk" , Role = Role.Supervisor};
    private UserDTO rasmus = new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk", Role = Role.Supervisor };
    
        
    //TagDTOs
    private TagDTO math = new TagDTO { Id = 1, Value = "Math Theory"};
    private TagDTO sql = new TagDTO { Id = 2, Value = "SQL" };
    private TagDTO goLang = new TagDTO { Id = 3, Value = "GoLang" };
    private TagDTO secondYear = new TagDTO { Id = 4, Value = "2nd Year Project" };
    private TagDTO spring22 = new TagDTO { Id = 5, Value = "Spring 2022"};
    //ProjectDTOs
    private ProjectDTO mathProjectDTO = new ProjectDTO { Id = 1, Name = "Math Project", Description = "Prove a lot of stuff.", Tags = new List<TagDTO>() { new TagDTO { Id = 1, Value = "Math Theory"} }, Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 2, Name = "Birgit", Email = "birgit@itu.dk" } } };
    private ProjectDTO databaseProjectDTO = new ProjectDTO { Id = 2, Name = "Database Project", Description = "Host a database with Docker.", Tags = new List<TagDTO>() {  new TagDTO { Id = 5, Value = "Spring 2022"}, new TagDTO { Id = 2, Value = "SQL" } }, Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 3, Name = "Bjørn", Email = "bjorn@itu.dk" } } };
    private ProjectDTO goProjectDTO = new ProjectDTO { Id = 3, Name = "Go Project", Description = "Create gRPC methods and connect it to SERF.", Tags = new List<TagDTO>() { new TagDTO { Id = 3, Value = "GoLang" } }, Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 1, Name = "Marco", Email = "marco@itu.dk" } } };
    private ProjectDTO secondYearProjectDTO = new ProjectDTO { Id = 4, Name = "Second Year Project", Description = "Group project in larger groups with a company.", Tags = new List<TagDTO>() { new TagDTO { Id = 4, Value = "2nd Year Project" }, new TagDTO { Id = 5, Value = "Spring 2022"} }, Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 4, Name = "Paolo", Email = "paolo@itu.dk" }, new UserDTO { Id = 5, Name = "Rasmus", Email = "rasmus@itu.dk" } } };
    // -

    [Fact]
    public async Task Get_IReadOnlyCollection_no_filter_returns_Projects()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);

        
        //Act
        var response = await client.GetFromJsonAsync<IReadOnlyCollection<ProjectDTO>>("api/Project/");
        
        //Assert
        Assert.Collection(response, actualProjectDTO => Assert.Collection(actualProjectDTO.Supervisors, Supervisors => Assert.Equal(birgit, Supervisors)),
            actualProjectDTO => Assert.Equal(databaseProjectDTO, actualProjectDTO),
            actualProjectDTO => Assert.Equal(goProjectDTO, actualProjectDTO),
            actualProjectDTO => Assert.Equal(secondYearProjectDTO, actualProjectDTO));
    }
    
    [Fact]
    public async Task Get_IReadOnlyCollection_with_no_matching_filter_returns_empty_list()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        var filterString = "tagIds=1&supervisorIds=1"; // Filters for tag "Math Theory" and supervisor "Marco"

        //Act
        var response = await client.GetFromJsonAsync<IReadOnlyCollection<ProjectDTO>>("api/Project?" + filterString);
        
        //Assert
        Assert.Empty(response);
    }
    
    [Fact]
    public async Task Get_IReadOnlyCollection_with_matching_filter_returns_matching_project()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        var filterString = "tagIds=2&tagIds=5"; // Filters for tags "SQL" and "Spring 2022"

        //Act
        var response = await client.GetFromJsonAsync<IReadOnlyCollection<ProjectDTO>>("api/Project?" + filterString);
        
        //Assert
        Assert.Equal(new List<ProjectDTO>(){databaseProjectDTO}, response);
    }

    [Fact]
    public async Task Get_from_id_returns_correct_Project()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
            
        //Act
        var response = await client.GetAsync($"api/Project/{1}");
        var actualProjectDTO = await response.Content.ReadFromJsonAsync<ProjectDTO>();
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(mathProjectDTO, actualProjectDTO);
        
    }

    [Fact]
    public async Task Get_with_nonexistant_id_returns_NotFound()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        
        //Act 
        var response = await client.GetAsync($"api/Project/{968}");
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }


    [Fact]
    public async Task Post_creates_new_Project()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);

        var projectCreateDTO = new ProjectCreateDTO()
        {
            Name = "New project 1", Description = "New project for integration testing",
            ExistingTagIds = new HashSet<int>() {1, 2},
            UserIds = new HashSet<int>(){1},
            NewTagDTOs = new HashSet<TagCreateDTO>()
        };
        var expectedProjectDTO = new ProjectDTO()
        {
            Id = 5,
            Name = "New project 1", Description = "New project for integration testing",
            Tags = new HashSet<TagDTO>() {math, sql},
            Supervisors = new HashSet<UserDTO>() {marco}
        };
        
        
        //Act
        var response = await client.PostAsJsonAsync($"api/Project/", projectCreateDTO);
        var createdProjectDTO = await client.GetFromJsonAsync<ProjectDTO>($"api/Project/{5}");
        
        //Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(expectedProjectDTO, createdProjectDTO);

    }

    [Fact]
    public async Task Post_Project_with_new_tags_create_new_tags()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);

        var projectCreateDTO = new ProjectCreateDTO()
        {
            Name = "New project 2", 
            Description = "New project for integration testing - add nonexistant tags",
            ExistingTagIds = new HashSet<int>(), 
            NewTagDTOs = new HashSet<TagCreateDTO>()
            {
                new TagCreateDTO() {Value = "C#", TagGroupId = 2},
                new TagCreateDTO() {Value = "Fall 2022", TagGroupId = 1}
            },
            UserIds = new HashSet<int>() {1}
            
        };

        var expectedProjectDTO = new ProjectDTO()
        {
            Id = 5,
            Name = "New project 2",
            Description = "New project for integration testing - add nonexistant tags",
            Tags = new List<TagDTO>()
            {
                new TagDTO(){Id = 6, Value = "C#"},
                new TagDTO(){Id = 7, Value = "Fall 2022"}
            },
            Supervisors = new HashSet<UserDTO>() {marco}
        };
        
        
        //Act
        var createResponse = await client.PostAsJsonAsync("api/Project/", projectCreateDTO);
        var getResponse = await client.GetAsync($"api/Project/{5}");
        var actualProjectDTO = await getResponse.Content.ReadFromJsonAsync<ProjectDTO>();

        //Assert 
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.Equal(expectedProjectDTO,actualProjectDTO);
    }
    
    [Fact]
    public async Task Post_with_unfilled_required_fields_returns_BadRequest()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);

        var faultyProjectCreateDTO = new ProjectCreateDTO()
        {
            Name = "",
            Description = "",
            ExistingTagIds = new HashSet<int>(){1},
            NewTagDTOs = new HashSet<TagCreateDTO>(),
            UserIds = new HashSet<int>(){1}
            
        };

        //Act 
        var response = await client.PostAsJsonAsync("api/Project/", faultyProjectCreateDTO);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    }

    [Fact]
    public async Task Delete_from_id_deletes_Project()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        
        //Act
        var response = await client.DeleteAsync($"api/Project/{1}");
        var notFoundResponse = await client.GetAsync($"api/Project/{1}");
        
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, notFoundResponse.StatusCode);
    }

    [Fact]
    public async Task Delete_from_nonexistent_id_returns_NotFound()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);

        //Act
        var response = await client.DeleteAsync($"api/Project/{10}");
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Updates_Correctly()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);

        var projectUpdateDTO = new ProjectUpdateDTO()
        {
            Id = 1,
            Name = "Project Updated",
            Description = "This project has been updated",
            ExistingTagIds = new HashSet<int>() {1,2,3},
            NewTagDTOs = new HashSet<TagCreateDTO>()
            {
                new TagCreateDTO() {Value = "Java", TagGroupId = 2}
            },
            UserIds = new HashSet<int>(){1}
        };

        var expectedProjectDTO = new ProjectDTO()
        {
            Id = 1,
            Name = "Project Updated",
            Description = "This project has been updated",
            Tags = new List<TagDTO>()
            {
                goLang,
                new TagDTO() {Id = 6, Value = "Java"},
                math,
                sql
            },
            Supervisors = new HashSet<UserDTO>()
            {
                marco, //Marco was able to edit another supervisors project (and was thus automatically added to it) without being initially involved as we are testing WithAdminClaims - however marco would not be able to do this in our actual project if he was a supervisor
                birgit
            }
        };

        //Act
        var response = await client.PutAsJsonAsync($"api/Project/{projectUpdateDTO.Id}", projectUpdateDTO);
        var getResponse = await client.GetAsync($"api/Project/{expectedProjectDTO.Id}");
        var actualUpdatedProjectDTO = await getResponse.Content.ReadFromJsonAsync<ProjectDTO>();
        
        //Assert
        Assert.Equal(HttpStatusCode.NoContent,response.StatusCode);
        Assert.Equal(HttpStatusCode.OK,getResponse.StatusCode);
        Assert.Equal(expectedProjectDTO,actualUpdatedProjectDTO);

    }

    [Fact]
    public async Task Update_nonexistant_id_returns_notFound()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        
        var projectUpdateDTO = new ProjectUpdateDTO()
        {
            Id = 10,
            Name = "Project Updated",
            Description = "This project has been updated",
            ExistingTagIds = new HashSet<int>() {1,2,3},
            NewTagDTOs = new HashSet<TagCreateDTO>()
            {
                new TagCreateDTO() {Value = "Java", TagGroupId = 2}
            },
            UserIds = new HashSet<int>(){1}
        };
        //Act
        var response = await client.PutAsJsonAsync($"api/Project/{projectUpdateDTO.Id}",projectUpdateDTO);
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_with_unfilled_required_fields_returns_BadRequest() 
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        
        var projectUpdateDTO = new ProjectUpdateDTO()
        {
            Id = 1,
            Name = "",
            Description = "",
            ExistingTagIds = new HashSet<int>() {1,2,3},
            NewTagDTOs = new HashSet<TagCreateDTO>()
            {
                new TagCreateDTO() {Value = "Java", TagGroupId = 2}
            },
            UserIds = new HashSet<int>(){1}
        };
        //Act
        var response = await client.PutAsJsonAsync($"api/Project/{projectUpdateDTO.Id}",projectUpdateDTO);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    
    [Fact]
    public async Task Update_with_tags_from_concurrently_deleted_tagGroup_returns_badRequest() 
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        
        var projectUpdateDTO = new ProjectUpdateDTO()
        {
            Id = 1,
            Name = "Project Updated",
            Description = "This project has been updated",
            ExistingTagIds = new HashSet<int>() {1,2,3},
            NewTagDTOs = new HashSet<TagCreateDTO>()
            {
                new TagCreateDTO() {Value = "Java", TagGroupId = 2}
            },
            UserIds = new HashSet<int>(){1}
        };
        
        //Act
        var deleteTagGroupResponse = await client.DeleteAsync($"api/TagGroup/{2}");  //Deletes "programming language" tag group, which tag 2 and 3 are part of
        var updateResponse = await client.PutAsJsonAsync($"api/Project/{projectUpdateDTO.Id}", projectUpdateDTO);
        
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteTagGroupResponse.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);
    }
}