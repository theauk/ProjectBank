namespace ProjectBank.Core.IRepositories;

public interface IUniversityRepository
{
    Task<Response> CreateAsync(UniversityCreateDTO university);
    Task<Response> UpdateAsync(string universityDomain, UniversityUpdateDTO university);
    Task<Response> DeleteAsync(string universityDomain);
    Task<Option<UniversityDTO?>> ReadAsync(string universityDomain);
}