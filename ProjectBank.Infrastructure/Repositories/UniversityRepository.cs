namespace ProjectBank.Infrastructure.Repositories;

public class UniversityRepository : IUniversityRepository
{
    private readonly IProjectBankContext _context;

    public UniversityRepository(IProjectBankContext context)
    {
        _context = context;
    }

    public async Task<Response> CreateAsync(UniversityCreateDTO university)
    {
        var entity = new University {DomainName = university.DomainName.ToLower()};

        _context.Universities.Add(entity);

        await _context.SaveChangesAsync();

        return Response.Created;
    }

    public async Task<Response> UpdateAsync(string universityDomain, UniversityUpdateDTO university)
    {
        throw new NotImplementedException();
    }

    public async Task<Response> DeleteAsync(string universityDomain)
    {
        var entity = await _context.Universities.FirstOrDefaultAsync(u => u.DomainName == universityDomain);

        if (entity == null)
        {
            return Response.NotFound;
        }

        _context.Universities.Remove(entity);
        await _context.SaveChangesAsync();

        return Response.Deleted;
    }

    public async Task<Option<UniversityDTO?>> ReadAsync(string universityDomain)
    {
        var entity = await _context.Universities.FirstOrDefaultAsync(u => u.DomainName == universityDomain);

        return entity == null
            ? null
            : new UniversityDTO
            {
                DomainName = entity.DomainName,
                Users = entity.Users.Select(u => new UserDTO {Id = u.Id, Name = u.Name}).ToHashSet(),
                Projects = entity.Projects.Select(p => new ProjectDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Supervisors = p.Supervisors.Select(u => new UserDTO {Id = u.Id, Name = u.Name}).ToHashSet(),
                    Tags = p.Tags.Select(t => new TagDTO {Id = t.Id, Value = t.Value}).OrderBy(t => t.Value).ToList()
                }).ToHashSet(),
                TagGroups = entity.TagGroups.Select(tg => new TagGroupDTO
                {
                    Id = tg.Id,
                    Name = tg.Name,
                    TagLimit = tg.TagLimit,
                    SupervisorCanAddTag = tg.SupervisorCanAddTag,
                    RequiredInProject = tg.RequiredInProject,
                    TagDTOs = tg.Tags.Select(t => new TagDTO {Id = t.Id, Value = t.Value}).OrderBy(t => t.Value)
                        .ToList()
                }).ToHashSet()
            };
    }
}