using System.Collections.Generic;
using ProjectBank.Core.DTOs;

namespace ProjectBank.Core.IRepositories;

public interface IProjectRepository 
{
    Task<(Response, int)> CreateAsync(ProjectCreateDTO project);
    Task<Response> UpdateAsync(int projectId, ProjectUpdateDTO project);
    Task<Response> DeleteAsync(int projectId);
    Task<(Response, ProjectDTO)> ReadAsync(int projectId);
    Task<(Response, IReadOnlyCollection<ProjectDTO>)> ReadAllAsync();
    Task<(Response, IReadOnlyCollection<ProjectDTO>)> ReadFilteredAsync(IEnumerable<int> tagIds);
}