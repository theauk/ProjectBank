using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Blazorise.Extensions;
using Microsoft.AspNetCore.Mvc;
using ProjectBank.Core.DTOs;
using Xunit.Abstractions;

namespace ProjectBank.Server.Integration.Tests;
public class TagGroupTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public TagGroupTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_IReadOnlyCollection_returns_TagGroups()
    {
        
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        
            //Tag Groups
        var semester = new TagGroupDTO
        {
            Id = 1,
            Name = "Semester",
            RequiredInProject = true,
            SupervisorCanAddTag = false,
            TagLimit = 2,
            TagDTOs = new List<TagDTO>() {new TagDTO(){Id = 5, Value = "Spring 2022"}}
        };
        var programmingLanguage = new TagGroupDTO
        {
            Id = 2,
            Name = "Programming Language",
            RequiredInProject = false,
            SupervisorCanAddTag = true,
            TagLimit = 10,
            TagDTOs = new List<TagDTO>() { new TagDTO(){Id = 2, Value = "SQL"},
                                           new TagDTO(){Id = 3, Value = "GoLang"}}
        };
        var mandatoryProjects = new TagGroupDTO
        {
            Id = 3,
            Name = "Mandatory Project",
            RequiredInProject = false,
            SupervisorCanAddTag = false,
            TagLimit = 1,
            TagDTOs = new List<TagDTO>() {new TagDTO(){Id = 4, Value = "2nd Year Project"}}
        };
        var topic = new TagGroupDTO
        {
            Id = 4,
            Name = "Topic",
            RequiredInProject = false,
            SupervisorCanAddTag = true,
            TagLimit = 10,
            TagDTOs = new List<TagDTO>() {new TagDTO(){Id = 1, Value = "Math Theory"}}
        };
        
        //Act
        var response = await client.GetFromJsonAsync<IReadOnlyCollection<TagGroupDTO>>("api/TagGroup");
        
        
        //Assert
        Assert.NotNull(response);
        Assert.Collection(response, actualTG => Assert.Equal(semester,actualTG),
            actualTG => Assert.Equal(programmingLanguage, actualTG),
            actualTG => Assert.Equal(mandatoryProjects,actualTG),
            actualTG => Assert.Equal(topic, actualTG)
        );
    }

    [Fact]
    public async Task Get_from_id_returns_correct_TagGroup() // Currently isn't used in our application frontend
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        
        var programmingLanguage = new TagGroupDTO
        {
            Id = 2,
            Name = "Programming Language",
            RequiredInProject = false,
            SupervisorCanAddTag = true,
            TagLimit = 10,
            TagDTOs = new List<TagDTO>() { new TagDTO(){Id = 2, Value = "SQL"},
                new TagDTO(){Id = 3, Value = "GoLang"}}
        };
    
        //Act
        var response = await client.GetAsync($"api/TagGroup/{2}");
        var actual = await response.Content.ReadFromJsonAsync<TagGroupDTO>();
        //Assert
        Assert.Equal( HttpStatusCode.OK,response.StatusCode);
        Assert.Equal(programmingLanguage, actual); 
    }

    [Fact]
    public async Task Get_with_nonexistant_id_returns_NotFound()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        
        //Act
        var response = await client.GetAsync($"api/TagGroup/{200}");
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_creates_new_TagGroup()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        
        var languageCreateDto = new TagGroupCreateDTO()
        {
            Name = "Language",
            RequiredInProject = true,
            SupervisorCanAddTag = true,
            TagLimit = 2,
            NewTagsDTOs = new HashSet<TagCreateDTO>() { new TagCreateDTO(){ Value = "Danish", TagGroupId = 5},
                new TagCreateDTO(){ Value = "English", TagGroupId = 5}}
        };
        var languageDto = new TagGroupDTO()
        {
            Id = 5,
            Name = "Language",
            RequiredInProject = true,
            SupervisorCanAddTag = true,
            TagLimit = 2,
            TagDTOs = new List<TagDTO>() { new TagDTO(){ Id = 6 ,Value = "Danish"},
                new TagDTO(){ Id = 7, Value = "English"}}
        };
        
        
        //Act
        var response = await client.PostAsJsonAsync("api/TagGroup", languageCreateDto);
        
        var createdResponse = await client.GetAsync($"api/TagGroup/{5}");
        var created = await createdResponse.Content.ReadFromJsonAsync<TagGroupDTO>();
        
        //Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        Assert.Equal(HttpStatusCode.OK, createdResponse.StatusCode);
        Assert.Equal(languageDto, created);
    }

    [Fact]
    public async Task Post_with_name_as_emptyString_returns_BadRequest()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        
        var language = new TagGroupDTO
        {
            Id = 5,
            Name = ""
            //other required values will be set to default (false or 0)
        };
        
        //Act
        var response = await client.PostAsJsonAsync("api/TagGroup", language);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Post_with_illegal_contents_returns_BadRequest()
    {
        //For futher
        //Mht. hvis RequiredInProject så skal der være mindst 1 tag / eller det skal håndteres i frontend / eller begge
    }
    
    [Fact]
    public async Task Delete_from_id_deletes_tagGroup()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        
        //Act
        var deleteResponse = await client.DeleteAsync($"api/TagGroup/{1}");
        var getResponse = await client.GetAsync($"api/TagGroup/{1}");
        
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        
    }

    [Fact]
    public async Task Delete_from_nonexistent_id_returns_NotFound()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);

        //Act
        var deleteResponse = await client.DeleteAsync($"api/TagGroup/{10}");
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task Updates_Correctly()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        
        var updateDTO = new TagGroupUpdateDTO()
            {
                Id = 2,
                Name = "Programming Language",
                RequiredInProject = false,
                SupervisorCanAddTag = true,
                TagLimit = 10,
                SelectedTagValues = new HashSet<string>(){"SQL"}
            };
        
        //Act
        var response = await client.PutAsJsonAsync($"api/TagGroup/{updateDTO.Id}", updateDTO);
        var updated = await client.GetFromJsonAsync<TagGroupDTO>($"api/TagGroup/{updateDTO.Id}");
        
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal((new TagGroupDTO()
            {
                Id = 2,
                Name = "Programming Language",
                RequiredInProject = false,
                SupervisorCanAddTag = true,
                TagLimit = 10,
                TagDTOs = new List<TagDTO>() {new TagDTO(){Id = 2, Value = "SQL"}}
            }),updated
        );
    }

    [Fact]
    public async Task Update_nonexistant_id_returns_notFound()
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        
        var updateDTO = new TagGroupUpdateDTO()
        {
            Id = 2,
            Name = "Programming Language",
            RequiredInProject = false,
            SupervisorCanAddTag = true,
            TagLimit = 10,
            SelectedTagValues = new HashSet<string>(){"SQL"}
        };
        
        //Act
        var response = await client.PutAsJsonAsync($"api/TagGroup/{362}", updateDTO);
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_with_name_as_emptyString_returns_BadRequest() //Bliver pt ikke håndteret på en måde, så en (dum) bruger får at vide hvad problemet er
    {
        //Arrange
        var provider = TestClaimsProvider.WithAdminClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        
        var updateDTO = new TagGroupUpdateDTO()
        {
            Id = 2,
            Name = "", 
            RequiredInProject = false,
            SupervisorCanAddTag = true,
            TagLimit = 10,
            SelectedTagValues = new HashSet<string>(){"SQL"}
        };
        
        //Act
        var response = await client.PutAsJsonAsync($"api/TagGroup/{2}", updateDTO);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
