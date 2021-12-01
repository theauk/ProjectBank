namespace Core;

public interface IUniversityRepository
{
    Task<(Response, UniversityDTO)> CreateAsync(UniversityCreateDTO university);
    Task<Response> UpdateAsync(int universityId, UniversityUpdateDTO university);
    Task<Response> DeleteAsync(int universityId);
    Task<(Response, UniversityDTO)> ReadAsync(int tagId);
    Task<(Response, IReadOnlyCollection<UniversityDTO>)> ReadAllAsync();
}