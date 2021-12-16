namespace ProjectBank.Core.IRepositories;

public interface IProjectRepository 
{
    Task<Response> CreateAsync(ProjectCreateDTO project, string ownerEmail);
    Task<Response> UpdateAsync(int projectId, ProjectUpdateDTO project);
    Task<Response> DeleteAsync(int projectId);
    Task<Option<ProjectDTO>> ReadAsync(int projectId);
    Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync();
    Task<IReadOnlyCollection<ProjectDTO>> ReadFilteredAsync(IList<int> tagIds, IList<int> supervisorIds);
}
