using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProjectBank.Infrastructure.Entities;
using ProjectBank.Infrastructure.Repositories;

namespace ProjectBank.Infrastructure.Tests
{
    public class UniversityRepositoryTests : IDisposable
    {
        private readonly ProjectBankContext _context;
        private readonly UniversityRepository _repository;
        private bool disposedValue;

        public UniversityRepositoryTests()
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
            var databases = new Tag { Id = 5, Value = "Databases" };

            var spring22 = new Tag { Id = 5, Value = "Spring 2022" };

            // TagGroups
            var subject = new TagGroup { Id = 1, Name = "Subject", RequiredInProject = true, SupervisorCanAddTag = true, TagLimit = 10, Tags = new HashSet<Tag>(){ math, databases } };
            var semester = new TagGroup { Id = 2, Name = "Semester", RequiredInProject = true, SupervisorCanAddTag = true, TagLimit = 4, Tags = new HashSet<Tag>(){ spring22 } };
            var language = new TagGroup { Id = 3, Name = "Programming Language", RequiredInProject = true, SupervisorCanAddTag = true, TagLimit = 10, Tags = new HashSet<Tag>() { sql, goLang } };
            var projects = new TagGroup { Id = 4, Name = "Semester Project", RequiredInProject = false, SupervisorCanAddTag = true, TagLimit = 5, Tags = new HashSet<Tag>() { secondYear } };

            // Projects
            var mathProject = new Project { Id = 1, Name = "Math Project", Description = "Prove a lot of stuff.", Tags = new HashSet<Tag>() { math }, Supervisors = new HashSet<User>() { birgit } };
            var databaseProject = new Project { Id = 2, Name = "Database Project", Description = "Host a database with Docker.", Tags = new HashSet<Tag>() { sql, spring22 }, Supervisors = new HashSet<User>() { bjorn } };
            var goProject = new Project { Id = 3, Name = "Go Project", Description = "Create gRPC methods and connect it to SERF.", Tags = new HashSet<Tag>() { goLang }, Supervisors = new HashSet<User>() { marco } };
            var secondYearProject = new Project { Id = 4, Name = "Second Year Project", Description = "Group project in larger groups with a company.", Tags = new HashSet<Tag>() { secondYear, spring22 }, Supervisors = new HashSet<User>() { paolo, rasmus } };

            // Universities
            // var itu = new University { DomainName = "itu", Users = new HashSet<User>(){ marco, birgit }};
            // var dtu = new University { DomainName = "dtu" };
            // var sdu = new University { DomainName = "sdu" };
            // -------------------


            context.SaveChanges();
            _context = context;
            _repository = new UniversityRepository(_context);
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