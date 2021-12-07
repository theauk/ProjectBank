using System.Linq;

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
            var entity = new Project
            {
                Name = project.Name,
                Description = project.Description,
                Tags = await GetTagsAsync(project.TagIds).ToSetAsync(),
                Supervisors = await GetUsersAsync(project.UserIds).ToSetAsync()
            };

            _context.Projects.Add(entity);

            await _context.SaveChangesAsync();

            return (Response.Created, new ProjectDTO {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Tags = entity.Tags.Select(t => new TagDTO { Id = t.Id, Value = t.Value }).ToHashSet(),
                Supervisors = entity.Supervisors.Select(s => new UserDTO { Id = s.Id, Name = s.Name }).ToHashSet()
            });
        }

        public async Task<Response> DeleteAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<(Response, ProjectDTO)> ReadAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<(Response, IReadOnlyCollection<ProjectDTO>)> ReadFilteredAsync(IEnumerable<int> tagIds)
        {
            throw new NotImplementedException();
        }

        public async Task<(Response, IReadOnlyCollection<ProjectDTO>)> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Response> UpdateAsync(int projectId, ProjectUpdateDTO project)
        {
            throw new NotImplementedException();
        }

        private async IAsyncEnumerable<Tag> GetTagsAsync(IEnumerable<int> tagIds)
        {
            var existing = await _context.Tags.Where(t => tagIds.Contains(t.Id)).ToDictionaryAsync(t => t.Id);

            foreach (var tagId in tagIds)
            {
                yield return existing.TryGetValue(tagId, out var t) ? t : new Tag{Value = t.Value};
            }
        }

        private async IAsyncEnumerable<User> GetUsersAsync(IEnumerable<int> userIds)
        {
            var existing = await _context.Users.Where(u => userIds.Contains(u.Id)).ToDictionaryAsync(u => u.Id);

            foreach (var userId in userIds)
            {
                yield return existing.TryGetValue(userId, out var u) ? u : null;
            }
        }
    }
}
