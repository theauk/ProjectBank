

namespace ProjectBank.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IProjectBankContext _context;

        public ProjectRepository(IProjectBankContext context)
        {
            _context = context;
        }

        public async Task<Response> CreateAsync(ProjectCreateDTO project)
        {
            var entity = new Project
            {
                Name = project.Name,
                Description = project.Description,
                Tags = await GetTagsAsync(project.ExistingTagIds).ToSetAsync(),
                Supervisors = await GetUsersAsync(project.UserIds).ToSetAsync()
            };

            if (entity.Supervisors.Contains(null))
            {
                return (Response.BadRequest);
            }

            _context.Projects.Add(entity);

            await _context.SaveChangesAsync();

            return Response.Created;
        }

        public async Task<Response> DeleteAsync(int projectId)
        {
            var entity = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            if (entity == null)
            {
                return Response.NotFound;
            }

            _context.Projects.Remove(entity);
            await _context.SaveChangesAsync();

            return Response.Deleted;
        }

        public async Task<Option<ProjectDTO?>> ReadAsync(int projectId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            
            return project == null ? null : new ProjectDTO 
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Tags = project.Tags.Select(t => new TagDTO { Id = t.Id, Value = t.Value }).ToHashSet(),
                Supervisors = project.Supervisors.Select(u => new UserDTO { Id = u.Id, Name = u.Name }).ToHashSet()
            };
        }

        

        public async Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync()
        {
            var projects = (await _context.Projects.Select(p => new ProjectDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Tags = p.Tags.Select(t => new TagDTO { Id = t.Id, Value = t.Value }).ToHashSet(),
                Supervisors = p.Supervisors.Select(user => new UserDTO { Id = user.Id, Name = user.Name }).ToHashSet()
            }).ToListAsync()).AsReadOnly();

            return projects;
        }

        public async Task<IReadOnlyCollection<ProjectDTO>> ReadFilteredAsync(IEnumerable<int> tagIds)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> UpdateAsync(int projectId, ProjectUpdateDTO project)
        {
            var entity = await _context.Projects.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == projectId);

            if (entity == null)
            {
                return Response.NotFound;
            }

            entity.Name = project.Name;
            entity.Description = project.Description;
            entity.Tags = await GetTagsAsync(project.ExistingTagIds).ToSetAsync();
            entity.Supervisors = await GetUsersAsync(project.UserIds).ToSetAsync();

            if (entity.Supervisors.Contains(null))
            {
                return Response.BadRequest;
            }

            await _context.SaveChangesAsync();

            return Response.Updated;
        }

        private async IAsyncEnumerable<Tag> GetTagsAsync(IEnumerable<int> tagIds)
        {
            var existing = await _context.Tags.Where(t => tagIds.Contains(t.Id)).ToDictionaryAsync(t => t.Id);

            foreach (var tagId in tagIds)
            {
                yield return existing.TryGetValue(tagId, out var t) ? t : new Tag { Value = t.Value };
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
