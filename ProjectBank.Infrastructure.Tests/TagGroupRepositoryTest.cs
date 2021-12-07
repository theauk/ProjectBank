using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectBank.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Sqlite;
using ProjectBank.Core;
using ProjectBank.Core.DTOs;
using ProjectBank.Infrastructure.Entities;
using Xunit;

namespace ProjectBank.Infrastructure.Tests;

public class TagGroupRepositoryTest : IDisposable
{
    private readonly ProjectBankContext _context;
    private readonly TagGroupRepository _repository;
    private bool disposedValue;

    public TagGroupRepositoryTest()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);
        _context = new ProjectBankContext(builder.Options);
        _repository = new TagGroupRepository(_context);
        _context.Database.EnsureCreated();

        var numtheo = new Tag() {Id = 1, Value = "Number Theory"};
        var crypto = new Tag() {Id = 2, Value = "Cryptography"};
        var setTheo = new Tag() {Id = 3, Value = "Set Theory"};
        var regex = new Tag() {Id = 4, Value = "RegEx's"};
        var autom = new Tag() {Id = 5, Value = "Automatas"};

        var spring22 = new Tag() {Id = 12, Value = "Spring 2022"};
        var autumn22 = new Tag() {Id = 13, Value = "Autumn 2022"};
        var spring23 = new Tag() {Id = 14, Value = "Spring 2023"};
        var autumn23 = new Tag() {Id = 15, Value = "Autumn 2023"};

        var semester = new TagGroup()
        {
            Id = 31,
            Name = "Semester",
            Tags = new HashSet<Tag>() {spring22, spring23, autumn22, autumn23},
            SupervisorCanAddTag = false,
            RequiredInProject = false,
            TagLimit = 999,
        };
        
        var topic = new TagGroup()
        {
            Id = 32,
            Name = "Topic",
            Tags = new HashSet<Tag>() {numtheo, crypto,setTheo,regex,autom},
            SupervisorCanAddTag = true,
            RequiredInProject = false,
            TagLimit = 999,
        };

        var project = new Project()
        {
            Name = "Project project",
            Description = "Descripton",
            Id = 100,
            Tags = new HashSet<Tag>() {numtheo}
        };

        _context.TagGroups.AddRange(semester, topic);
        _context.Projects.AddRange(project);
        _context.SaveChanges();

    }

    [Fact]
    public async Task ReadAsync_finds_TagGroup_with_id_31()
    {
        //Arrange
        var spring22 = new TagDTO() {Id = 12, Value = "Spring 2022"};
        var autumn22 = new TagDTO() {Id = 13, Value = "Autumn 2022"};
        var spring23 = new TagDTO() {Id = 14, Value = "Spring 2023"};
        var autumn23 = new TagDTO() {Id = 15, Value = "Autumn 2023"};
        var tags = new HashSet<TagDTO>() {spring22, spring23, autumn22, autumn23};


        // Act
        var found = await _repository.ReadAsync(31);
        
        //Assert
        Assert.Equal(31, found.Value.Id);
        Assert.Equal("Semester", found.Value.Name);
        Assert.Equal(tags.Count, found.Value.TagDTOs.Count); //Ser om de har samme længde
        foreach (var expected in tags)
        {
            Assert.True(found.Value.TagDTOs.Any(actual => actual.Value == expected.Value && actual.Id == expected.Id));   
        }
        Assert.False(found.Value.SupervisorCanAddTag);
        Assert.False(found.Value.RequiredInProject);
        Assert.Equal(999, found.Value.TagLimit);
    }
/*
    [Fact]
    public async Task ReadAsync_on_nonexistant_id_throws_InvalidOperationException()
    {
            //TODO Fix/Spørgsmål: Hvordan fungere det med InvalidOperationException i Options?
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.ReadAsync(56));
    }
*/
    [Fact]
    public async Task CreateAsync_creates_new_TagGroup_with_generated_id()
    {
        //Arrange
        var taggroup = new TagGroupCreateDTO()
        {
            Name = "Level",
            TagCreateDTOs = new HashSet<TagCreateDTO>()
            {
                new TagCreateDTO() {Value = "Bachelor"},
                new TagCreateDTO() {Value = "Master"},
                new TagCreateDTO() {Value = "PhD"}
            },
            SupervisorCanAddTag = false,
            RequiredInProject = false,
            TagLimit = 999,
        };
        
        //act
        var created  = await _repository.CreateAsync(taggroup);
        var readAllTagGroups = await _repository.ReadAllAsync();
        var createdTagGroupDtO = readAllTagGroups.FirstOrDefault(tgDTO => tgDTO.Name == taggroup.Name); //TODO Need implementation: Kræver at vi checker efter duplicate tag groups for at være trolig
        
        
        //Assert
        Assert.Equal(Response.Created, created);
        Assert.Equal(taggroup.Name, createdTagGroupDtO.Name);
        Assert.Equal(taggroup.RequiredInProject, createdTagGroupDtO.RequiredInProject);
        Assert.Equal(taggroup.SupervisorCanAddTag, createdTagGroupDtO.SupervisorCanAddTag);
        Assert.Equal(taggroup.TagLimit, createdTagGroupDtO.TagLimit);
        Assert.Equal(taggroup.TagCreateDTOs.Count, createdTagGroupDtO.TagDTOs.Count);
        
        foreach (var expected in taggroup.TagCreateDTOs)
        {
            Assert.True(createdTagGroupDtO.TagDTOs.Any(actual => actual.Value == expected.Value));   
        }
    }
/*
    [Fact]
    public async Task CreateAsync_create_duplicate_returns_duplicate_response()
    {
        //TODO implementér den her
        // Lidt uden for scope, men kunne være relevant - er ikke blevet implementeret endnu
        throw new NotImplementedException();
    }
*/
    [Fact]
    public async Task DeleteAsync_deletes_tagGroup_with_Id_32()
    {
        var deleted = await _repository.DeleteAsync(32);
        Assert.Equal(Response.Deleted, deleted);
        Assert.Null( _context.TagGroups.Find(100));
    }
    
    [Fact]
    public async Task DeleteAsync_delete_nonexistant_returns_NotFound()
    {
        var notFound = await _repository.DeleteAsync(-1);
        Assert.Equal(Response.NotFound, notFound);
    }

    
    
    [Fact]
    public async Task Update_TagGroup_31_with_required_bool_new_tags_new_name()
    {
        //Arrange

        var tagsToDelete = new HashSet<int>() {12};
        var spring23 = new TagCreateDTO() {Value = "spring 23"};
        var tagsToAdd = new HashSet<TagCreateDTO>() {spring23};
        var tagGroupUpdate = new TagGroupUpdateDTO()
            {Name = "Semester (Updated)", SupervisorCanAddTag = false, RequiredInProject = true, DeletedTagIds = tagsToDelete, NewTags = tagsToAdd};

        
        // Act
        var update =  await _repository.UpdateAsync
            (31, tagGroupUpdate);
        var readUpdatedTagGroup = await _repository.ReadAsync(31);
        var updatedTags = readUpdatedTagGroup.Value.TagDTOs;
        
        //Assert
        Assert.Equal(Response.Updated, update);
        Assert.False(readUpdatedTagGroup.Value.SupervisorCanAddTag);
        Assert.True(readUpdatedTagGroup.Value.RequiredInProject);
        Assert.Equal("Semester (Updated)", readUpdatedTagGroup.Value.Name);
        Assert.True(updatedTags.Any(t => t.Id != 12 && t.Value != "Spring 2022"));
        Assert.True(updatedTags.Any(t => t.Value == "Spring 2023"));
    }

    [Fact]
    public async Task Update_TagGroupID_56_returns_NotFound()
    {
        //Arrange
        var tagsToDelete = new HashSet<int>() {12};
        var spring23 = new TagCreateDTO() {Value = "spring 23"};
        var tagsToAdd = new HashSet<TagCreateDTO>() {spring23};
        var tagGroupUpdate = new TagGroupUpdateDTO()
            {Name = "Semester (Updated)", SupervisorCanAddTag = false, RequiredInProject = true, DeletedTagIds = tagsToDelete, NewTags = tagsToAdd};
    
        // Act
        var notFound =  await _repository.UpdateAsync
            (56, tagGroupUpdate);
        
        Assert.Equal(Response.NotFound,notFound);
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}