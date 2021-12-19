namespace ProjectBank.Core.IRepositories;

public interface IUniversityRepository
{
    Task<Response> CreateAsync(UniversityCreateDTO university);
    Task<Response> DeleteAsync(string domain);
    Task<Option<UniversityDTO>> ReadAsync(string domain);
    Task<IReadOnlyCollection<UniversityDTO>> ReadAllAsync();
}
