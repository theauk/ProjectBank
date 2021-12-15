namespace ProjectBank.Infrastructure.Tests;

public class UniversityRepositoryTests : RepoTests
{
    private readonly UniversityRepository _repository;

    public UniversityRepositoryTests() => _repository = new UniversityRepository(_context);
}
