using ProjectBank.Core.DTOs;

namespace ProjectBank.Core.IRepositories;

public interface IUniversityRepository
{
    Task<Response> CreateAsync(UniversityCreateDTO university);
    Task<Response> UpdateAsync(int universityId, UniversityUpdateDTO university);
    Task<Response> DeleteAsync(int universityId);
    Task<Option<UniversityDTO?>> ReadAsync(int universityId);
}