using ProjectBank.Core.DTOs;

namespace ProjectBank.Core.IRepositories;

public interface IProjectRepository 
{
    Task<(Response, ProjectDTO)> CreateAsync(ProjectCreateDTO project);
    Task<Response> UpdateAsync(int projectId, ProjectUpdateDTO project);
    Task<Response> DeleteAsync(int projectId);
    Task<(Response, ProjectDTO)> ReadAsync(int projectId);
    Task<(Response, IReadOnlyCollection<ProjectDTO>)> ReadFilteredAsync(IEnumerable<int> tagIds);
    Task<(Response, IReadOnlyCollection<ProjectDTO>)> ReadAllAsync();
}