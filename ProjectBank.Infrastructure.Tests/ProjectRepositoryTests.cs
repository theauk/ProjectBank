using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectBank.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using ProjectBank.Core;
using ProjectBank.Core.DTOs;
using ProjectBank.Infrastructure.Entities;
using Xunit;

namespace ProjectBank.Infrastructure.Tests
{
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
                UserIds = new HashSet<int> { 4, 5 }
            };

            var response = await _repository.CreateAsync(project, "paolo@itu.dk", "test");
            var actualProject = (await _repository.ReadAsync(4)).Value;

            Assert.Equal(Response.Created, response);
            Assert.Equal(4, actualProject.Id);
            Assert.Equal("Bachelor Project", actualProject.Name);
            Assert.Equal("The final project of SWU.", actualProject.Description);
            Assert.Equal(new List<int> { 1, }, actualProject.Tags.Select(t => t.Id));
            Assert.Equal(new List<int> { 4, 5, }, actualProject.Supervisors.Select(s => s.Id));
        }

        [Fact]
        public async Task CreateAsync_given_invalid_user_id_returns_null()
        {
            var project = new ProjectCreateDTO
            {
                Name = "Bachelor Project",
                Description = "The final project of SWU.",
                ExistingTagIds = new HashSet<int>() { 1 },
                UserIds = new HashSet<int>() { 4, 6 }
            };

            var actual = await _repository.CreateAsync(project, "test@itu.dk", "test");

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
            var project = (await _repository.ReadAsync(4)).Value;

            Assert.Equal(4, project.Id);
            Assert.Equal("Second Year Project", project.Name);
            Assert.Equal("Group project in larger groups with a company.", project.Description);
            Assert.Collection(project.Tags,
                tag => Assert.Equal(new TagDTO { Id = 4, Value = "2nd Year Project" }, tag),
                tag => Assert.Equal(new TagDTO { Id = 5, Value = "Spring 2022" }, tag)
            );
            Assert.Collection(project.Supervisors,
                supervisor => Assert.Equal(new UserDTO { Id = 4, Name = "Paolo" }, supervisor),
                supervisor => Assert.Equal(new UserDTO { Id = 5, Name = "Rasmus" }, supervisor)
            );
        }

        [Fact]
        public async Task ReadAsync_given_non_existing_id_returns_NotFound()
        {
            var actual = (await _repository.ReadAsync(33)).Value;

            Assert.Null(actual);
        }

        [Fact]
        public async Task ReadAllAsync_returns_all_projects()
        {
            var projects = await _repository.ReadAllAsync();

            Assert.Collection(projects,
                project => Assert.Equal(new ProjectDTO{ Id = 1, Name = "Math Project", Description = "Prove a lot of stuff.", Tags = new HashSet<TagDTO>() { new TagDTO{ Id = 1, Value = "Math Theory" } }, Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 2, Name = "Birgit" } }}, project),
                project => Assert.Equal(new ProjectDTO{ Id = 2, Name = "Database Project", Description = "Host a database with Docker.", Tags = new HashSet<TagDTO>() { new TagDTO { Id = 2, Value = "SQL", }, new TagDTO { Id = 5, Value = "Spring 2022" }}, Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 3, Name = "Bjørn" } }}, project),
                project => Assert.Equal(new ProjectDTO{ Id = 3, Name = "Go Project", Description = "Create gRPC methods and connect it to SERF.", Tags = new HashSet<TagDTO>() { new TagDTO { Id = 3, Value = "GoLang" } }, Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 1, Name = "Marco" } }}, project),
                project => Assert.Equal(new ProjectDTO{ Id = 4, Name = "Second Year Project", Description = "Group project in larger groups with a company.", Tags = new HashSet<TagDTO>() { new TagDTO { Id = 4, Value = "2nd Year Project" }, new TagDTO { Id = 5, Value = "Spring 2022" } }, Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 4, Name = "Paolo" }, new UserDTO { Id = 5, Name = "Rasmus" }}}, project)
            );
        }

        [Fact]
        public async Task UpdateAsync_updates_existing_character()
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
            Assert.Empty(mathProject.Tags);
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
                UserIds = new HashSet<int>() { 1, 7 }
            };

            var actual = await _repository.UpdateAsync(1, project);

            Assert.Equal(Response.BadRequest, actual);
        }
    }
}