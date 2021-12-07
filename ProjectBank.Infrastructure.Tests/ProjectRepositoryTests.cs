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


            Assert.Equal(5, actualProject.Id);
            Assert.Equal(Response.Created, actualResponse);
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