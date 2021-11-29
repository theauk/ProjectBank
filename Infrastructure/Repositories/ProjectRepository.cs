namespace Infrastructure
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectBankContext _context;

        public ProjectRepository(ProjectBankContext context)
        {
            _context = context;
        }

        public Task<(Response, ProjectDTO)> CreateAsync(ProjectCreateDTO project)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<(Response, IReadOnlyCollection<ProjectDTO>)> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<(Response, ProjectDTO)> ReadAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(int projectId, ProjectUpdateDTO project)
        {
            throw new NotImplementedException();
        }
    }
}
