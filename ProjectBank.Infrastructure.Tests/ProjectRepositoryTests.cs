using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectBank.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using ProjectBank.Core;
using ProjectBank.Core.DTOs;
using Xunit;

namespace ProjectBank.Infrastructure.Tests
{
    public class ProjectRepositoryTests : IDisposable
    {
        private readonly ProjectBankContext _context;
        private readonly ProjectRepository _repository;
        private bool disposedValue;

        public ProjectRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var builder = new DbContextOptionsBuilder<ProjectBankContext>();
            builder.UseSqlite(connection);

            var context = new ProjectBankContext(builder.Options);
            context.Database.EnsureCreated();

            // --- Test data ---
            // Supervisors
            var marco = new User { Id = 1, Name = "Marco" };
            var birgit = new User { Id = 2, Name = "Birgit" };
            var bjorn = new User { Id = 3, Name = "Bjørn" };
            var paolo = new User { Id = 4, Name = "Paolo" };
            var rasmus = new User { Id = 5, Name = "Rasmus" };

            // Tags
            var math = new Tag { Id = 1, Value = "Math Theory" };
            var sql = new Tag { Id = 2, Value = "SQL" };
            var goLang = new Tag { Id = 3, Value = "GoLang" };
            var secondYear = new Tag { Id = 4, Value = "2nd Year Project" };

            var spring22 = new Tag { Id = 5, Value = "Spring 2022" };

            // Projects
            var mathProject = new Project { Id = 1, Name = "Math Project", Description = "Prove a lot of stuff.", Tags = new HashSet<Tag>() { math }, Supervisors = new HashSet<User>() { birgit } };
            var databaseProject = new Project { Id = 2, Name = "Database Project", Description = "Host a database with Docker.", Tags = new HashSet<Tag>() { sql, spring22 }, Supervisors = new HashSet<User>() { bjorn } };
            var goProject = new Project { Id = 3, Name = "Go Project", Description = "Create gRPC methods and connect it to SERF.", Tags = new HashSet<Tag>() { goLang }, Supervisors = new HashSet<User>() { marco } };
            var secondYearProject = new Project { Id = 4, Name = "Second Year Project", Description = "Group project in larger groups with a company.", Tags = new HashSet<Tag>() { secondYear, spring22 }, Supervisors = new HashSet<User>() { paolo, rasmus } };
            // -------------------

            context.Projects.AddRange(
                mathProject,
                databaseProject,
                goProject,
                secondYearProject
            );

            context.Users.AddRange(
                marco,
                birgit,
                bjorn,
                paolo,
                rasmus
            );

            context.SaveChanges();
            _context = context;
            _repository = new ProjectRepository(_context);
        }

        [Fact]
        public async Task CreateAsync_creates_new_project_with_generated_id()
        {
            var project = new ProjectCreateDTO
            {
                Name = "Bachelor Project",
                Description = "The final project of SWU.",
                TagIds = new HashSet<int>() { 1 },
                UserIds = new HashSet<int>() { 4, 5 }
            };

            var actual = await _repository.CreateAsync(project);
            var actualResponse = actual.Item1;
            var actualProject = actual.Item2;

            Assert.Equal(Response.Created, actualResponse);
            Assert.Equal(5, actualProject.Id);
            Assert.Equal("Bachelor Project", actualProject.Name);
            Assert.Equal("The final project of SWU.", actualProject.Description);
            Assert.Collection(actualProject.Tags,
                tag => Assert.Equal(new TagDTO { Id = 1, Value = "Math Theory" }, tag)
            );
            Assert.Collection(actualProject.Supervisors,
                supervisor => Assert.Equal(new UserDTO { Id = 4, Name = "Paolo" }, supervisor),
                supervisor => Assert.Equal(new UserDTO { Id = 5, Name = "Rasmus" }, supervisor)
            );
        }

        [Fact]
        public async Task CreateAsync_given_invalid_user_id_returns_null()
        {
            var project = new ProjectCreateDTO
            {
                Name = "Bachelor Project",
                Description = "The final project of SWU.",
                TagIds = new HashSet<int>() { 1 },
                UserIds = new HashSet<int>() { 4, 6 }
            };

            var actual = await _repository.CreateAsync(project);

            Assert.Equal((Response.BadRequest, null), actual);
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
            var actual = await _repository.ReadAsync(4);
            var project = actual.Item2;

            Assert.Equal(Response.Success, actual.Item1);
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
            var actual = await _repository.ReadAsync(33);

            Assert.Equal((Response.NotFound, null), actual);
        }

        // [Fact]
        // public async Task ReadAllAsync_returns_all_projects()
        // {
        //     var actual = await _repository.ReadAllAsync();
        //     var projects = actual.Item2;

        //     Assert.Equal(Response.Success, actual.Item1);
        //     Assert.Collection(actual.Item2, 
        //         project => Assert.Equal(new ProjectDTO { Id = 1, Name = "Math Project", Description = "Prove a lot of stuff.", Tags = new HashSet<TagDTO>() { new TagDTO { Id = 1, Value = "Math Theory" } }, Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 2, Name = "Birgit" } } }, project),
        //         project => Assert.Equal(new ProjectDTO { Id = 2, Name = "Database Project", Description = "Host a database with Docker.", Tags = new HashSet<TagDTO>() { new TagDTO { Id = 2, Value = "SQL" }, new TagDTO { Id = 5, Value = "Spring 2022" } }, Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 3, Name = "Bjørn" } } }, project),
        //         project => Assert.Equal(new ProjectDTO { Id = 3, Name = "Go Project", Description = "Create gRPC methods and connect it to SERF.", Tags = new HashSet<TagDTO>() { new TagDTO { Id = 3, Value = "GoLang" } }, Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 1, Name = "Marco" } } }, project),
        //         project => Assert.Equal(new ProjectDTO { Id = 4, Name = "Second Year Project", Description = "Group project in larger groups with a company.", Tags = new HashSet<TagDTO>() { new TagDTO { Id = 4, Value = "2nd Year Project" }, new TagDTO { Id = 5, Value = "Spring 2022" } }, Supervisors = new HashSet<UserDTO>() { new UserDTO { Id = 4, Name = "Paolo" }, new UserDTO { Id = 5, Name = "Rasmus" } } }, project)
        //     );
        // }

        [Fact]
        public async Task UpdateAsync_updates_existing_character()
        {
            var project = new ProjectUpdateDTO
            {
                  Name = "Extreme Math Project",
                  Description = "Prove even harder stuff.",
                  TagIds = new HashSet<int>(),
                  UserIds = new HashSet<int>() { 1, 2 }
            };

            var actual = await _repository.UpdateAsync(1, project);

            Assert.Equal(Response.Updated, actual);

            var mathProject = await _repository.ReadAsync(1);
            
            Assert.Equal(Response.Success, mathProject.Item1);
            
            Assert.Empty(mathProject.Item2.Tags);
        }

        [Fact]
        public async Task UpdateAsync_given_non_existing_id_returns_NotFound()
        {
            var project = new ProjectUpdateDTO
            {
                  Name = "Extreme Math Project",
                  Description = "Prove even harder stuff.",
                  TagIds = new HashSet<int>(){ 1 },
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
                  TagIds = new HashSet<int>(){ 1 },
                  UserIds = new HashSet<int>() { 1, 7 }
            };

            var actual = await _repository.UpdateAsync(1, project);

            Assert.Equal(Response.BadRequest, actual);
        }
            
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}