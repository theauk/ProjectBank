using Infrastructure;
using ProjectBank.Core.DTOs;
using ProjectBank.Core.IRepositories;

namespace ProjectBank.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IProjectBankContext _context;

        public ProjectRepository(IProjectBankContext context)
        {
            _context = context;
        }

        public async Task<(Response, ProjectDTO)> CreateAsync(ProjectCreateDTO project)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> DeleteAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<(Response, IReadOnlyCollection<ProjectDTO>)> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<(Response, ProjectDTO)> ReadAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> UpdateAsync(int projectId, ProjectUpdateDTO project)
        {
            throw new NotImplementedException();
        }
    }
}
