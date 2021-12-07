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

        var numtheo = new Tag("Number Theory") {Id = 1};
        var crypto = new Tag("Cryptography") {Id = 2};
        var setTheo = new Tag("Set Theory") {Id = 3};
        var regex = new Tag( "RegEx's") {Id = 4};
        var autom = new Tag("Automatas") {Id = 5};

        var spring22 = new Tag("Spring 2022") {Id = 12,};
        var autumn22 = new Tag("Autumn 2022") {Id = 13};
        var spring23 = new Tag("Spring 2023") {Id = 14};
        var autumn23 = new Tag("Autumn 2023") {Id = 15};

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
        var tagsExpected = tags.GetEnumerator();


        // Act
        var found = await _repository.ReadAsync(31);
        var tagsActual = found.Tags.GetEnumerator();
        
        //Assert
        Assert.Equal(31, found.Id);
        Assert.Equal("Semester", found.Name);
        Assert.Equal(tags.Count, found.Tags.Count); //Ser om de har samme længde

        var currentGroupId = -1;
        while (tagsExpected.MoveNext())
        {
            if (tagsExpected.Current.Id != currentGroupId)
            {
                tagsActual.MoveNext();
                currentGroupId = tagsActual.Current.Id;
            }
            Assert.Equal(tagsExpected.Current.Id, tagsActual.Current.Id); // Lidt redundant, men ok for at have en assert
            Assert.Equal(tagsExpected.Current.Value, tagsActual.Current.Value);
        }
        
        
        Assert.False(found.SupervisorCanAddTag);
        Assert.False(found.RequiredInProject);
        Assert.Equal(999, found.TagLimit);
        tagsActual.Dispose();
        tagsExpected.Dispose();
    }

    [Fact]
    public async Task ReadAsync_on_nonexistant_id_returns_null()
    {
        //TODO Overvej om der er brug for en Response.NotFound i stedet - con at det kræver at vi laver om på repo
        var notFound = await _repository.ReadAsync(56);
        
        Assert.Null(notFound);
    }

    [Fact]
    public async Task CreateAsync_creates_new_TagGroup_with_generated_id()
    {
        var taggroup = new TagGroupCreateDTO()
        {
            Name = "Level",
            Tags = new HashSet<TagCreateDTO>()
            {
                new TagCreateDTO("Bachelor"),
                new TagCreateDTO("Master"),
                new TagCreateDTO("PhD")
            },
            SupervisorCanAddTag = false,
            RequiredInProject = false,
            TagLimit = 999,
        };
        var created  = await _repository.CreateAsync(taggroup);
        Assert.Equal(Response.Created, created); 
        // Vi skal lave vores (I)TagGroupRepository om hvis vi skal teste createAsync ift. indholdet - men vi får ikke brug for det i praksis. - oli
        //TODO this - hvis vi laver repo om
    }
/*
    [Fact]
    public async Task CreateAsync_create_duplicate_returns_error()
    {
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
        var tagGroupUpdate = new TagGroupUpdateDTO()
            {Name = "Semester (Updated)", SupervisorCanAddTag = false, RequiredInProject = true};
        var tagsToDelete = new HashSet<int>() {12};
        var spring23 = new TagCreateDTO("spring 23");
        var tagsToAdd = new HashSet<TagCreateDTO>() {spring23};

        
        // Act
        var update =  await _repository.UpdateAsync
            (31, tagGroupUpdate ,tagsToDelete, tagsToAdd);
        var readUpdatedTagGroup = await _repository.ReadAsync(31);
        var updatedTags = readUpdatedTagGroup.Tags;
        
        //Assert
        Assert.Equal(Response.Updated, update);
        Assert.False(readUpdatedTagGroup.SupervisorCanAddTag);
        Assert.True(readUpdatedTagGroup.RequiredInProject);
        Assert.Equal("Semester (Updated)", readUpdatedTagGroup.Name);
        Assert.True(readUpdatedTagGroup.Tags.Any(t => t.Id != 12 && t.Value != "Spring 2022"));
        Assert.True(readUpdatedTagGroup.Tags.Any(t => t.Value == "Spring 2023"));
        
        
    }
    



    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                ReadAsync_finds_TagGroup_with_id_31().Dispose();
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