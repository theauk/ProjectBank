using ProjectBank.Core.DTOs;

namespace ProjectBank.Core.IRepositories;

public interface ITagRepository
{
    Task<Response> CreateAsync(TagCreateDTO tag);
    Task<Response> DeleteAsync(int tagId);
    Task<(Response, TagDTO?)> ReadAsync(int tagId);
    Task<(Response, IReadOnlyCollection<TagDTO>)> ReadAllAsync();
}