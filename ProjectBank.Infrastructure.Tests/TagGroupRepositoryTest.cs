using System;
using System.Collections.Generic;
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
        var context = new ProjectBankContext(builder.Options);
        context.Database.EnsureCreated();
    

     var numtheo = new Tag() { Id = 1, Value = "Number Theory",};
        var crypto = new Tag() {Id = 2, Value = "Cryptography",};
        var setTheo = new Tag() {Id = 3, Value = "Set Theory",};
        var regex = new Tag() {Id = 4, Value = "RegEx's",};
        var autom = new Tag() {Id = 5, Value = "Automatas",};

        var spring22 = new Tag() {Id = 12, Value = "Spring 2022",};
        var autumn22 = new Tag() {Id = 13, Value = "Autumn 2022",};
        var spring23 = new Tag() {Id = 14, Value = "Spring 2023",};
        var autumn23 = new Tag() {Id = 15, Value = "Autumn 2023",};

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

        context.TagGroups.AddRange(semester, topic);
        context.Projects.AddRange(project);

    }

    [Fact]
    public async Task CreateAsync_creates_new_TagGroup_with_generated_id()
    {
        var taggroup = new TagGroupCreateDTO()
        {
            Name = "Level",
            TagCreateDTOs = new HashSet<TagCreateDTO>()
            {
                new TagCreateDTO(){Value = "Bachelor"},
                new TagCreateDTO(){Value = "Master"},
                new TagCreateDTO(){Value = "PhD"}
            },
            SupervisorCanAddTag = false,
            RequiredInProject = false,
            TagLimit = 999,
        };
        var created  = await _repository.CreateAsync(taggroup);
        Assert.Equal(created, created);
        //TODO this 
    }

    [Fact]
    public async Task DeleteAsync_deletes_tagGroup_with_Id_32()
    {
        var deleted = await _repository.DeleteAsync(32);
        Assert.Equal(Response.Deleted, deleted);
        Assert.Equal(0, _context.Projects.Find(100).Tags.Count);
    }

    [Fact]
    public async Task Update_TagGroup_31_with_required_bool()
    {

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