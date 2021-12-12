using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProjectBank.Infrastructure.Entities;
using ProjectBank.Infrastructure.Repositories;

namespace ProjectBank.Infrastructure.Tests
{
    public class UniversityRepositoryTests : RepoTests
    {
        private readonly UniversityRepository _repository;

        public UniversityRepositoryTests() => _repository = new UniversityRepository(_context);
    }
}