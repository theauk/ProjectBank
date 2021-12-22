namespace ProjectBank.Infrastructure.Repositories;

public class UniversityRepository : IUniversityRepository
{
    private readonly IProjectBankContext _context;

    public UniversityRepository(IProjectBankContext context) => _context = context;

    public async Task<Response> CreateAsync(UniversityCreateDTO university)
    {
        // Check if university already exists
        var existing = (await ReadAsync(university.DomainName)).Value;
        if (existing != null)
            return Response.Conflict;

        var entity = new University
        {
            DomainName = university.DomainName
        };

        _context.Universities.Add(entity);

        await _context.SaveChangesAsync();

        return Response.Created;
    }

    public async Task<Response> DeleteAsync(string domain)
    {
        var entity = await _context.Universities.FindAsync(domain);

        if (entity == null)
            return Response.NotFound;

        _context.Universities.Remove(entity);
        await _context.SaveChangesAsync();

        return Response.Deleted;
    }

    public async Task<Option<UniversityDTO>> ReadAsync(string domain) => (await _context.Universities.FindAsync(domain))?.ToDTO();

    public async Task<IReadOnlyCollection<UniversityDTO>> ReadAllAsync() => (await _context.Universities.ToListAsync())
        .ToDTO().ToList().AsReadOnly();
}